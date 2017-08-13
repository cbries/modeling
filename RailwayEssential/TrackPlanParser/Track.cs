using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;

namespace TrackPlanParser
{
    public class Track : List<TrackInfo>, ITrackEdit
    {
        public void Remove(int x, int y)
        {
            var item = Get(x, y);
            base.Remove(item);
        }

        public void ChangeSymbol(int x, int y, int themeId)
        {
            var item = Get(x, y);

            if (item != null)
            {
                item.ThemeId = themeId;
            }
            else
            {
                if (themeId <= 0)
                {
                    Remove(x, y);
                    return;
                }

                if (x < 0 || y < 0)
                {
                    Remove(x, y);
                    return;
                }

                Add(new TrackInfo()
                {
                    Description = "",
                    ThemeId = themeId,
                    Orientation = "rot0",
                    X = x,
                    Y = y
                });
            }
        }

        public void RotateSymbol(int x, int y, string orientation)
        {
            var item = Get(x, y);
            if (item != null)
                item.Orientation = orientation;
        }

        public JArray GetJson()
        {
            JArray ar = new JArray();
            foreach (var item in this)
            {
                TrackInfo info = item as TrackInfo;
                if (info == null)
                    continue;

                if (item.ThemeId <= 0)
                    continue;

                if (info.X < 0 || info.Y < 0)
                    continue;

                ar.Add(info.ToObject());

            }
            return ar;
        }

        public TrackInfo Get(int x, int y)
        {
            if (x < 0)
                return null;
            if (y < 0)
                return null;

            foreach (var e in this)
            {
                if (e == null)
                    continue;

                if (e.X == x && e.Y == y)
                    return e;
            }

            return null;
        }
    }
}
