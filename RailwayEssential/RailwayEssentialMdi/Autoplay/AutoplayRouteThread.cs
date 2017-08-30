using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;
using RailwayEssentialMdi.ViewModels;
using TrackInformation;
using TrackInformationCore;
using TrackPlanParser;
using TrackWeaver;

namespace RailwayEssentialMdi.Autoplay
{
    public class AutoplayRouteThread
    {
        public RailwayEssentialModel Model { get; set; }
        public TrackWeaver.TrackWeaver Weaver => Model.Dispatcher.Weaver;
        public Analyze.Route Route { get; set; }
        public TrackInfo SrcBlock { get; private set; }
        public TrackInfo DestBlock { get; private set; }
        public Task Task { get; private set; }
        public Autoplay Autoplayer { get; set; }

        private CancellationTokenSource _cts;
        private CancellationToken _tkn;

        private static readonly string Prefix = "       HandleBlockRoute():";

        public static AutoplayRouteThread Start(RailwayEssentialModel model, Autoplay autoplayer, Analyze.Route route)
        {
            var c = new AutoplayRouteThread
            {
                Model = model,
                Autoplayer = autoplayer,
                Route = route
            };

            c.Initialize();

            return c;
        }

        private void Initialize()
        {
            var firstItem = Route.First();
            var lastItem = Route.Last();

            if (firstItem == lastItem)
                return;

            if (firstItem == null || lastItem == null)
                return;

            SrcBlock = Model.TrackEntity.Track.Get(firstItem.X, firstItem.Y);
            DestBlock = Model.TrackEntity.Track.Get(lastItem.X, lastItem.Y);
        }

        public bool IsRunning
        {
            get
            {
                if (_cts == null)
                    return false;
                if (_tkn == null)
                    return false;
                if (Task.Status == TaskStatus.Running
                    || Task.Status == TaskStatus.WaitingToRun
                  )
                    return true;
                if (Task.Status == TaskStatus.Canceled)
                    return false;
                return false;
            }
        }

        private Dictionary<string, string> GetBlockData(TrackInfo destBlock)
        {
            if (destBlock == null)
                return null;

            Trace.WriteLine($"{Prefix} Destination {destBlock}");

            var eventSpec = DestBlock.GetOption("events");
            JObject events = null;
            if (!string.IsNullOrEmpty(eventSpec))
                events = JObject.Parse(eventSpec);
            if (events == null)
                return null;

            Dictionary<string, string> results = new Dictionary<string, string>();

              string[] sensors = new string[3];
            string[] eventNames = new string[3];
            for (int i = 0; i < 3; ++i)
            {
                if (events[$"sensor{i}"] != null)
                    sensors[i] = events[$"sensor{i}"].ToString();

                if (events[$"event{i}"] != null)
                    eventNames[i] = events[$"event{i}"].ToString();

                if (!string.IsNullOrEmpty(sensors[i]))
                {
                    if (results.ContainsKey(sensors[i]))
                    {
                        // ignore
                    }
                    else
                    {
                        results.Add(sensors[i], eventNames[i]);
                    }
                }
            }

            return results;
        }

        private class ItemData
        {
            public Analyze.Route Route { get; set; }
            public TrackInfo Info { get; set; }
            public IItem Item { get; set; }
            public TrackInformation.Switch ItemSwitch => Item as TrackInformation.Switch;
            public S88 ItemS88 => Item as S88;
            public Func<TrackCheckResult> S88Checker { get; set; }
            public bool S88HasBeenHandled { get; set; }
            
            public bool IsS88 => Item is S88;
            public bool IsSwitch => Item is TrackInformation.Switch;
            public bool HasSwitchTurn
            {
                get
                {
                    if (!IsSwitch)
                        return false;

                    foreach (var r in Route)
                    {
                        if (r == null)
                            continue;

                        if (r.X != Info.X)
                            continue;

                        if (r.Y != Info.Y)
                            continue;

                        if (!Globals.SwitchIds.Contains(r.ThemeId))
                            return false;

                        return r.HasTurn;
                    }

                    return false;
                }
            }

            public string DestBlockEvent { get; set; }
        }

        private ItemData Get(List<ItemData> items, int x, int y)
        {
            if (items == null || items.Count == 0)
                return null;
            if (x == -1 || y == -1)
                return null;

            foreach (var it in items)
            {
                if (it == null)
                    continue;
                if (it.Info == null)
                    continue;
                if (it.Info.X == x && it.Info.Y == y)
                    return it;
            }

            return null;
        }

        public void Start()
        {
            if (_cts != null)
                return;

            _cts = new CancellationTokenSource();
            _tkn = _cts.Token;

            Task = new Task(() =>
            {
                // **********************************************************************
                // ** Route Thread 
                // **********************************************************************

                bool isRouteSet = false; // flag initialization of route's handling thread
                Locomotive locObject = null; // the current locomotive on the route
                List<ItemData> routeData = new List<ItemData>(); // s88 and switches on the route 
                
                for (;;)
                {
                    if (!isRouteSet)
                    {
                        Route.IsBusy = true;
                        isRouteSet = true;
                        Autoplayer?.SetRoute(Route, true);
                        int locObjectId = -1;
                        if (Autoplayer != null)
                            locObjectId = Autoplayer.GetLocObjectIdOfRoute(Route);
                        locObject = Model.Dispatcher.GetDataProvider().GetObjectBy(locObjectId) as Locomotive;

                        if (locObject != null)
                            Trace.WriteLine($"{Prefix} Locomotive: {locObject.Name}");

                        TrackWeaveItems weaverItems = new TrackWeaveItems();
                        var weaveFilepath = Path.Combine(Model.Project.Dirpath, Model.Project.Track.Weave);
                        if (!weaverItems.Load(weaveFilepath))
                            throw new Exception("Reading weave file failed.");

                        Dictionary<TrackInfo, List<IItem>> trackObjects = new Dictionary<TrackInfo, List<IItem>>();

                        foreach (var pt in Route)
                        {
                            if (pt == null)
                                continue;

                            var item = Model.TrackEntity.Track.Get(pt.X, pt.Y);
                            if (item == null)
                                continue;

                            var itemObjects = Model.Dispatcher.Weaver.GetObject(item);
                            if (itemObjects.Count == 0)
                                continue;

                            if (trackObjects.ContainsKey(item))
                                trackObjects[item].AddRange(itemObjects);
                            else
                                trackObjects.Add(item, itemObjects);
                        }

                        #region DEBUG route's track info
                        Trace.WriteLine($"{Prefix} Route's track infos:");
                        foreach (var info in trackObjects.Keys)
                        {
                            var objs = trackObjects[info];

                            Trace.Write($"{Prefix} {info}: ");
                            foreach (var o in objs)
                                Trace.Write($"{o.ObjectId}, ");
                            Trace.WriteLine("||");
                        }
                        #endregion

                        Dictionary<TrackInfo, S88> s88TrackObjects = new Dictionary<TrackInfo, S88>();
                        Dictionary<TrackInfo, TrackInformation.Switch> switchTrackObjects = new Dictionary<TrackInfo, TrackInformation.Switch>();

                        #region prepare route data

                        foreach (var trackInfo in trackObjects.Keys)
                        {
                            var itemObjects = trackObjects[trackInfo];
                            if (itemObjects.Count == 0)
                                continue;

                            foreach (var obj in itemObjects)
                            {
                                if (obj == null)
                                    continue;

                                if (obj is S88)
                                    s88TrackObjects.Add(trackInfo, obj as S88);

                                if(obj is TrackInformation.Switch)
                                    switchTrackObjects.Add(trackInfo, obj as TrackInformation.Switch);
                            }
                        }

                        foreach (var trackInfo in s88TrackObjects.Keys)
                        {
                            var s88Obj = s88TrackObjects[trackInfo];

                            var data = new ItemData
                            {
                                Route = Route,
                                Info = trackInfo,
                                Item = s88Obj,
                                S88Checker = Weaver.GetCheckFnc(s88Obj, trackInfo)
                            };

                            routeData.Add(data);
                        }

                        foreach (var trackInfo in switchTrackObjects.Keys)
                        {
                            var switchObj = switchTrackObjects[trackInfo];

                            var data = new ItemData
                            {
                                Route = Route,
                                Info = trackInfo,
                                Item = switchObj
                            };

                            routeData.Add(data);
                        }

                        var sensorsAndEvents = GetBlockData(DestBlock);
                        if (sensorsAndEvents != null)
                        {
                            foreach (var sensorName in sensorsAndEvents.Keys)
                            {
                                var eventName = sensorsAndEvents[sensorName];

                                TrackInfo sensorTrackInfo = null;

                                foreach (var item in Model.TrackEntity.Track)
                                {
                                    if (item == null || string.IsNullOrEmpty(item.Name))
                                        continue;

                                    if (item.Name.Equals(sensorName))
                                    {
                                        sensorTrackInfo = item;
                                        break;
                                    }
                                }

                                if (sensorTrackInfo != null)
                                {
                                    var it = Get(routeData, sensorTrackInfo.X, sensorTrackInfo.Y);

                                    if (it == null)
                                    {
                                        var s88Obj = Helper.GetObject(Model.Dispatcher, Model.TrackEntity.Track, sensorTrackInfo.X, sensorTrackInfo.Y);

                                        var data = new ItemData
                                        {
                                            Route = Route,
                                            Info = sensorTrackInfo,
                                            Item = s88Obj,
                                            DestBlockEvent = eventName
                                        };

                                        routeData.Add(data);
                                    }
                                    else
                                    {
                                        it.DestBlockEvent = eventName;
                                    }

                                    Trace.WriteLine($"{Prefix} Sensor({sensorName}) with Event({eventName})");
                                }
                            }
                        }

                        #endregion

                        #region set switches to let the locomotive pass the route

                        foreach (var data in routeData)
                        {
                            if (data == null || !data.IsSwitch || data.ItemSwitch == null)
                                continue;

                            var sw = data.ItemSwitch;
                            var v = data.HasSwitchTurn ? 0 : 1;
                            if (sw.InvertCommand)
                            {
                                if (v == 1) v = 0;
                                else v = 1;
                            }
                            var vs = v == 1 ? "TURN" : "STRAIGHT";
                            Trace.WriteLine($"{Prefix} Switch '{sw.Name1}' change to '{vs}'");
                            sw.ChangeDirection(v);
                        }

                        #endregion

                        if (locObject != null)
                        {
                            locObject.ChangeDirection(false);
                            locObject.ChangeSpeed(Locomotive.SpeedNormal);
                        }
                    }

                    var s = SrcBlock.ToString().Replace(" ", "");
                    var d = DestBlock.ToString().Replace(" ", "");
                    Trace.WriteLine($"{Prefix} {s}  TO  {d}");

                    foreach (var s88data in routeData)
                    {
                        if (s88data == null || !s88data.IsS88)
                            continue;

                        var obj = s88data.ItemS88;
                        if (obj == null)
                            continue;

                        if (s88data.S88HasBeenHandled)
                            continue;

                        var weavedItem = Helper.GetWeaveItem(Model.Dispatcher, s88data.Info.X, s88data.Info.Y);
                        if(weavedItem != null)
                            Trace.WriteLine(" #0 Pin " + weavedItem.Pin);

                        Trace.WriteLine(" #1 " + s88data.ItemSwitch.Name1);
                        Trace.WriteLine(" #2 " + s88data.ItemSwitch.Addr);
                        Trace.WriteLine(" #2 " + s88data.ItemSwitch.State);
                        
                        bool state = false;
                        try
                        {
                            var bb = s88data.S88Checker();
                            if (bb != null && bb.State != null)
                                state = bb.State.Value;
                        }
                        catch(Exception ex)
                        {
                            Trace.WriteLine($"{Prefix} {ex.Message}");
                        }

                        if (state)
                        {
                            s88data.S88HasBeenHandled = true;

                            Trace.WriteLine($"{Prefix} {s88data.Info} {obj} state '{state}' -> {s88data.DestBlockEvent}");

                            string evName = s88data.DestBlockEvent;
                            if (!string.IsNullOrEmpty(evName))
                            {
                                var stopped = false;

                                if (evName.Equals("enter", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (locObject != null)
                                        locObject.ChangeSpeed(Locomotive.SpeedBlockEntered);

                                    Trace.WriteLine($"{Prefix} New speed {Locomotive.SpeedBlockEntered} for {locObject.Name}");
                                }
                                else if (evName.Equals("enter in", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (locObject != null)
                                    {
                                        var currentSpeed = locObject.Speed;
                                        var secondsToStop = 3;
                                        int speedSteps = currentSpeed / secondsToStop;

                                        var o = locObject;

                                        var task = Task.Run(() =>
                                        {
                                            currentSpeed -= speedSteps;

                                            if (currentSpeed <= 0)
                                            {
                                                o?.ChangeSpeed(Locomotive.SpeedStop);
                                            }
                                            else
                                            {
                                                o?.ChangeSpeed(currentSpeed);
                                            }

                                            Trace.WriteLine($"{Prefix} Loc speed {locObject.Name} is {currentSpeed}");

                                            Thread.Sleep(1 * 1000);
                                        });

                                        task.Wait();

                                        stopped = true;
                                    }
                                }
                                else if (evName.Equals("in", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (locObject != null)
                                        locObject.ChangeSpeed(Locomotive.SpeedStop);

                                    Trace.WriteLine($"{Prefix} STOP {Locomotive.SpeedBlockEntered}");

                                    stopped = true;
                                }

                                if (stopped)
                                {
                                    // move loc from source block to destination block
                                    // reset current route
                                    // be ready for next routing decision

                                    SrcBlock.SetOption("blockCurrentLocomotive", "");
                                    DestBlock.SetOption("blockCurrentLocomotive", $"{locObject.ObjectId}");
                                    Route.IsBusy = false;
                                    Route.StartBusiness = DateTime.MaxValue;
                                    Route.StopBusiness = DateTime.Now;
                                }
                            }
                        }
                    }

                    #region Thread stuff

                    Thread.Sleep(1 * 500);

                    if (_tkn.IsCancellationRequested)
                    {
                        Trace.WriteLine($"{Prefix} Stop requested...");
                        Route.IsBusy = false;
                        return;
                    }

                    #endregion
                }

                // **********************************************************************
                // ** END Route Thread 
                // **********************************************************************

            }, _tkn);

            Task?.Start();
        }

        public void Stop(bool waitFor=false)
        {
            try
            {
                if (!_tkn.CanBeCanceled)
                    return;

                if (_cts == null)
                    return;

                if (!_cts.IsCancellationRequested)
                {
                    _cts.Cancel();

                    if (waitFor)
                    {

                        bool r = Task.Wait(30 * 1000);
                        if (!r)
                            throw new Exception("Can not stop thread for Route: " + Route);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Execution of Task failed: " + ex.Message);
            }
            finally
            {
                
            }
        }

        public void Cleanup()
        {
            try
            {
                _cts?.Dispose();
                _cts = null;
            }
            catch
            {
                // ignore
            }

            try
            {
                Task?.Dispose();
                Task = null;
            }
            catch
            {
                // ignore
            }
        }
    }
}