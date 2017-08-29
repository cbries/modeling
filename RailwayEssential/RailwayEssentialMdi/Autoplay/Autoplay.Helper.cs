using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Effects;
using RailwayEssentialMdi.Analyze;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        public void SetRoute(Analyze.Route route, bool state)
        {
            if (route == null)
                return;

            var n = route.Count;

            var trackEntity = Ctx.TrackEntity;

            for (int idx = 1; idx < n - 1; ++idx)
            {
                var r = route[idx];
                if (r == null)
                    continue;

                if (trackEntity?.Viewer != null)
                {
                    string themeIcon = null;

                    var trackInfo = trackEntity.Track.Get(r.X, r.Y);
                    if (trackInfo != null)
                    {
                        var themeInfo = Theme?.Get(trackInfo.ThemeId);
                        if (themeInfo != null)
                        {
                            if (state)
                                themeIcon = themeInfo.Off.Route;
                            else
                                themeIcon = themeInfo.Off.Default;
                        }
                    }

                    if (!string.IsNullOrEmpty(themeIcon))
                    {
                        var x = trackInfo.X;
                        var y = trackInfo.Y;
                        var themeId = trackInfo.ThemeId;
                        var orientation = trackInfo.Orientation;
                        var symbol = themeIcon;

                        var isSwitch = RailwayEssentialCore.Globals.SwitchIds.Contains(themeId);

                        if (r.HasTurn && isSwitch)
                        {
                            var parts = symbol.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
                            if(parts.Length == 2)
                                symbol = parts[0] + "-t-" + parts[1];
                            else if (parts.Length == 1)
                                symbol = parts[0] + "-t";
                        }

                        trackEntity.Viewer.ExecuteJs($"changeSymbol({x}, {y}, {themeId}, \"{orientation}\", \"{symbol}\");");
                    }
                }
            }
        }

        private AutoplayRouteThread GetByRoute(Analyze.Route route)
        {
            foreach (var thread in _blockRouteThreads)
            {
                if (thread == null)
                    continue;

                if (thread.Route == route)
                    return thread;
            }

            return null;
        }

        private int GetLocObjectIdOfRoute(Analyze.Route route, bool destination = false)
        {
            if (!destination)
            {
                var startPoint = route.First();
                if (startPoint != null)
                {
                    var startItem = Ctx.TrackEntity.Track.Get(startPoint.X, startPoint.Y);
                    if (startItem != null)
                    {
                        var locObjectId = startItem.GetLocomotiveObjectId();
                        if (locObjectId != -1)
                            return locObjectId;
                    }
                }
            }
            else
            {
                var endPoint = route.Last();
                if (endPoint != null)
                {
                    var endItem = Ctx.TrackEntity.Track.Get(endPoint.X, endPoint.Y);
                    if (endItem != null)
                    {
                        var locObjectId = endItem.GetLocomotiveObjectId();
                        if (locObjectId != -1)
                            return locObjectId;
                    }
                }
            }

            return -1;
        }

        private List<RouteGroup> GetFreeBlockGroups()
        {
            List<Route> busyRoutes = new List<Route>();
            foreach (var r in Ctx.Project.BlockRoutes)
            {
                if (r == null)
                    continue;
                if (r.IsBusy)
                    busyRoutes.Add(r);
            }

            List<RouteGroup> grps = new List<RouteGroup>();
            foreach (var grp in Ctx.Project.BlockRouteGroups)
            {
                if (grp == null)
                    continue;

                foreach (var r0 in grp.Routes)
                {
                    foreach (var r1 in busyRoutes)
                    {
                        if(Route.Cross(r0, r1, true))
                            goto Outer;
                    }
                }

                grps.Add(grp);

                Outer:
                    continue;
            }
            
            return grps;
        }
    }
}
