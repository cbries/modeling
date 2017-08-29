using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RailwayEssentialMdi.ViewModels;
using TrackPlanParser;

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

                bool isRouteSet = false;

                string prefix = "       HandleBlockRoute():";

                for (;;)
                {
                    if (!isRouteSet)
                    {
                        isRouteSet = true;
                        Autoplayer?.SetRoute(Route, true);
                    }

                    var s = SrcBlock.ToString().Replace(" ", "");
                    var d = DestBlock.ToString().Replace(" ", "");

                    Trace.WriteLine($"{prefix} {s} to {d}");
                    
                    Thread.Sleep(1 * 1000);

                    if (_tkn.IsCancellationRequested)
                    {
                        Trace.WriteLine($"{prefix} Stop requested...");
                        Route.IsBusy = false;
                        return;
                    }
                }

                // **********************************************************************
                // ** END Route Thread 
                // **********************************************************************

            }, _tkn);

            if (Task != null)
                Task.Start();
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