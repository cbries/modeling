using System;
using System.Collections.Generic;

namespace RailwayEssentialMdi.Analyze
{
    public class WayPoints : List<MapItem>
    {
        public Map Ctx { get; private set; }

        public WayPoints()
        {

        }

        public WayPoints(Map ctx, string path)
        {
            Ctx = ctx;
            var parts = path.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pp in parts)
            {
                if (string.IsNullOrEmpty(pp))
                    continue;
                var m = pp.Trim().TrimStart('(').TrimEnd(')').Trim();
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
                    Add(item);
            }
        }

        public WayPoints Clone()
        {
            WayPoints pts = new WayPoints();
            foreach (var e in this)
                pts.Add(e);
            return pts;
        }
    }
}
