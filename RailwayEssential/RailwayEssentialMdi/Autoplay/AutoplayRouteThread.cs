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

        private CancellationTokenSource _cts;
        private CancellationToken _tkn;

        public static AutoplayRouteThread Start(RailwayEssentialModel model, Analyze.Route route)
        {
            var c = new AutoplayRouteThread { Model = model, Route = route };
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

        public void Start()
        {
            Trace.WriteLine("<RailwayEssential> Start route handling.");

            if (_cts != null)
                return;

            _cts = new CancellationTokenSource();
            _tkn = _cts.Token;

            Task = new Task(() =>
            {
                for (;;)
                {
                    try
                    {
                        if (_tkn.IsCancellationRequested)
                            _tkn.ThrowIfCancellationRequested();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        return;
                    }
                    finally
                    {
                        Route.IsBusy = false;
                    }

                    var s = SrcBlock.ToString().Replace(" ", "");
                    var d = DestBlock.ToString().Replace(" ", "");

                    Trace.WriteLine($"HandleBlockRoute():  {s} to {d}");

                    Thread.Sleep(5 * 1000);
                }

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
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}