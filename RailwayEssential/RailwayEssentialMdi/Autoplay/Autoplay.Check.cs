using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TrackInformation;
using TrackPlanParser;
using Route = RailwayEssentialMdi.Analyze.Route;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        private Track Track => Ctx.TrackEntity.Track;
        private DataProvider DataProvider => Ctx?.Dispatcher?.GetDataProvider();
        private Dispatcher.Dispatcher Dispatcher => Ctx?.Dispatcher;
        private Theme.Theme Theme => Ctx?.Theme;

        private readonly List<AutoplayRouteThread> _blockRouteThreads = new List<AutoplayRouteThread>();

        private readonly Random _rnd = new Random(DateTime.Now.Millisecond);

        private void InitializeRouteThreads()
        {
            int n = Ctx.Project.BlockRoutes.Count;
            if (n == 0)
                return;

            var blockRoutes = Ctx.Project.BlockRoutes;
            if (blockRoutes == null || blockRoutes.Count == 0)
                return;

            foreach (var route in blockRoutes)
            {
                if (route == null)
                    continue;

                var childThread = AutoplayRouteThread.Start(Ctx, route);
                _blockRouteThreads.Add(childThread);
            }
        }

        private void StopRouteThreads()
        {
            List<Task> tasks = new List<Task>();

            foreach (var t in _blockRouteThreads)
            {
                if (t == null)
                    continue;

                if(t.Task!= null)
                    tasks.Add(t.Task);

                t.Stop();
            }

            bool r = Task.WaitAll(tasks.ToArray(), 10 * 1000);
            if (!r)
                Ctx.LogError("Tasks can not be finished.");

            foreach (var t in _blockRouteThreads)
            {
                if (t == null || t.Task == null)
                    continue;

                try
                {
                    t.Task.Dispose();
                }
                catch
                {
                    // ignore
                }
            }

            _blockRouteThreads.Clear();
        }

        private void Check()
        {
            Trace.WriteLine($"{GetTimeStr()} ## Autoplay::Check()");

            if (Ctx == null || Ctx.Project == null)
                return;

            var grps = GetFreeBlockGroupsWithLocomotive();
            var grpsN = grps.Count;
            if (grpsN == 0)
            {
                // no route is free 

                return;
            }
            var grpsIdx = _rnd.Next(0, grpsN);

            var grp = grps[grpsIdx];

            if (grp != null)
            {
                List<Analyze.Route> routesWithLocs = new List<Route>();

                foreach (var r in grp.Routes)
                {
                    if(r == null)
                        continue;

                    var locObjectId = GetLocObjectIdOfRoute(r);
                    if (locObjectId != -1)
                        routesWithLocs.Add(r);
                }

                var routeN = routesWithLocs.Count;
                var routeIdx = _rnd.Next(0, routeN);

                var route = routesWithLocs[routeIdx];

                if (route != null)
                {
                    route.IsBusy = true;
                    route.StartBusiness = DateTime.Now;
                    route.StartBusiness = DateTime.MinValue;

                    var thread = GetByRoute(route);
                    thread?.Start();
                }
            }
        }        
    }
}
