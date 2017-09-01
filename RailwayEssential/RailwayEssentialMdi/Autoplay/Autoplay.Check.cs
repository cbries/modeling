/*
 * MIT License
 *
 * Copyright (c) 2017 Dr. Christian Benjamin Ries
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TrackInformation;
using Route = RailwayEssentialMdi.Analyze.Route;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        // seconds between a stop of a locomotive and next run
        // i.e. we just give other locomotives the chance to run
        private static int SecondsToNextLocRun = 10;

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

                var childThread = AutoplayRouteThread.Start(Ctx, this, route);

                _blockRouteThreads.Add(childThread);
            }
        }

        private List<Task> GetRunningRouteThreads()
        {
            List<Task> tasks = new List<Task>();

            foreach (var t in _blockRouteThreads)
            {
                if (t?.Task != null && t.IsRunning)
                    tasks.Add(t.Task);
            }

            return tasks;
        }

        private void StopRouteThreads()
        {
            List<Task> tasks = new List<Task>();

            foreach (var t in _blockRouteThreads)
            {
                if (t == null)
                    continue;

                if(t.Task != null)
                    tasks.Add(t.Task);

                var route = t.Route;
                if (route != null)
                    SetRoute(route, false);

                if(t.IsRunning)
                    t.Stop();
            }

            Trace.WriteLine($"Wait for {tasks.Count} Tasks...");

            bool r = Task.WaitAll(tasks.ToArray(), 30 * 1000);
            if (!r)
                Ctx.LogError("Tasks can not be finished.");

            foreach (var t in _blockRouteThreads)
            {
                if (t == null || t.Task == null)
                    continue;

                try
                {
                    t.Cleanup();
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
            if (Ctx == null || Ctx.Project == null)
                return;

            var grps = GetFreeBlockGroups();
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
                List<Route> routesWithLocs = new List<Route>();

                foreach (var r in grp.Routes)
                {
                    if(r == null)
                        continue;

                    var locObjectIdStart = GetLocObjectIdOfRoute(r);
                    var locObjectIdEnd = GetLocObjectIdOfRoute(r, true);

                    if (locObjectIdStart != -1 && locObjectIdEnd == -1 && !r.IsBusy)
                    {
                        if (Ctx.Dispatcher.GetDataProvider().GetObjectBy(locObjectIdStart) is Locomotive locObj)
                        {
                            TimeSpan duration = DateTime.Now - locObj.StopTime;
                            if (duration.Seconds > SecondsToNextLocRun)
                                routesWithLocs.Add(r);
                        }
                        else
                        {
                            routesWithLocs.Add(r);
                        }
                    }
                }

                var routeN = routesWithLocs.Count;
                if (routeN == 0)
                    return;
                var routeIdx = _rnd.Next(0, routeN);
                var route = routesWithLocs[routeIdx];

                if (route != null)
                {
                    Ctx?.LogAutoplay($"START Group {grp.GroupName} with Route {route}");
                    Trace.WriteLine($"START Group {grp.GroupName} with Route {route}");

                    GetByRoute(route)?.Start();
                }
            }
        }        
    }
}
