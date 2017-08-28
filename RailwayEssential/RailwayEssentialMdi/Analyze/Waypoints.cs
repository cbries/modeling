using System;
using System.Collections.Generic;
using RailwayEssentialCore;

namespace RailwayEssentialMdi.Analyze
{
    public class WayPoints : List<MapItem>
    {
        public Map Ctx { get; private set; }

        public WayPoints(Map ctx, string path)
        {
            Ctx = ctx;
            var parts = path.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pp in parts)
            {
                if (string.IsNullOrEmpty(pp))
                    continue;

                bool hasTurn = false;

                var m = pp.Trim();
                if (m.EndsWith(">", StringComparison.OrdinalIgnoreCase))
                {
                    hasTurn = true;
                    m = m.TrimEnd('>');
                }

                m = m.Trim().TrimStart('(').TrimEnd(')').Trim();
                if (string.IsNullOrEmpty(m))
                    continue;
                var mparts = m.Split(',');
                int x, y;
                if (!int.TryParse(mparts[0], out x))
                    x = -1;
                if (!int.TryParse(mparts[1], out y))
                    y = -1;

                var item = Ctx.Get(x, y);

                if (item != null)
                {
                    if (Globals.SwitchIds.Contains(item.ThemeId) && hasTurn)
                    {
                        var cpy = item.ShallowCopy();
                        cpy.HasTurn = hasTurn;
                        Add(cpy);
                    }
                    else
                    {
                        Add(item);
                    }
                }
            }
        }

        public Route ToRoute()
        {
            lock (this)
            {
                var wps = new Route();
                foreach (var item in this)
                {
                    if (item == null)
                        continue;

                    if (item.ThemeId == -1)
                        continue;

                    WayPoint wp = new WayPoint
                    {
                        X = item.Info.X, 
                        Y = item.Info.Y,
                        ThemeId = item.ThemeId,
                        Orientation = Helper.GetOrientation(item.Info),
                        HasTurn = item.HasTurn
                    };

                    wps.Add(wp);
                }
                return wps;
            }
        }
    }
}
