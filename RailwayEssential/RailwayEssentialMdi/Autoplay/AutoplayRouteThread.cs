using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RailwayEssentialCore;
using RailwayEssentialMdi.ViewModels;
using TrackInformation;
using TrackInformationCore;
using TrackPlanParser;
using Switch = TrackInformation.Switch;

namespace RailwayEssentialMdi.Autoplay
{
    public class AutoplayRouteThread
    {
        public RailwayEssentialModel Model { get; set; }
        public Analyze.Route Route { get; set; }
        public TrackInfo SrcBlock { get; private set; }
        public TrackInfo DestBlock { get; private set; }
        public Task Task { get; private set; }
        public Autoplay Autoplayer { get; set; }

        private CancellationTokenSource _cts;
        private CancellationToken _tkn;

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

                string prefix = "       HandleBlockRoute():";

                bool isRouteSet = false;
                int locObjectId = -1;
                Locomotive locObject = null;
                Dictionary<TrackInfo, S88> s88TrackObjects = new Dictionary<TrackInfo, S88>();
                Dictionary<TrackInfo, TrackInformation.Switch> switchTrackObjects = new Dictionary<TrackInfo, TrackInformation.Switch>();

                for (;;)
                {
                    if (!isRouteSet)
                    {
                        Route.IsBusy = true;
                        isRouteSet = true;
                        Autoplayer?.SetRoute(Route, true);
                        if(Autoplayer != null)
                            locObjectId = Autoplayer.GetLocObjectIdOfRoute(Route);
                        locObject = Model.Dispatcher.GetDataProvider().GetObjectBy(locObjectId) as Locomotive;

                        if (locObject != null)
                            Trace.WriteLine($"{prefix} Locomotive: {locObject.Name}");

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
                        Trace.WriteLine($"{prefix} Route's track infos:");
                        foreach (var info in trackObjects.Keys)
                        {
                            var objs = trackObjects[info];

                            Trace.Write($"{prefix} {info}: ");
                            foreach (var o in objs)
                                Trace.Write($"{o.ObjectId}, ");
                            Trace.WriteLine("||");
                        }
                        #endregion

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
                    }

                    var s = SrcBlock.ToString().Replace(" ", "");
                    var d = DestBlock.ToString().Replace(" ", "");
                    Trace.WriteLine($"{prefix} {s} to {d}");

                    #region Thread stuff

                    Thread.Sleep(1 * 1000);

                    if (_tkn.IsCancellationRequested)
                    {
                        Trace.WriteLine($"{prefix} Stop requested...");
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