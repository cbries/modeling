using System;
using System.Collections.Generic;
using System.Linq;
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

        private int GetLocObjectIdOfRoute(Analyze.Route route)
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

            return -1;
        }

        private List<RouteGroup> GetFreeBlockGroupsWithLocomotive()
        {
            List<RouteGroup> grps = new List<RouteGroup>();

            foreach (var grp in Ctx.Project.BlockRouteGroups)
            {
                if (grp == null || grp.Routes.Count == 0)
                    continue;

                bool addToFreeList = true;

                foreach (var r in grp.Routes)
                {
                    if (r == null)
                        continue;

                    if (r.IsBusy)
                    {
                        addToFreeList = false;

                        break;
                    }
                }

                if (!addToFreeList)
                    continue;

                addToFreeList = false;

                foreach (var r in grp.Routes)
                {
                    if (r == null)
                        continue;

                    var locObjectId = GetLocObjectIdOfRoute(r);
                    if (locObjectId != -1)
                    {
                        addToFreeList = true;
                        break;
                    }
                }

                if (addToFreeList)
                    grps.Add(grp);
            }

            return grps;
        }
    }
}
