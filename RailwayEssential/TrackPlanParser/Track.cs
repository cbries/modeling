﻿using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public class Track : List<TrackInfo>, ITrackEdit
    {
        public void ChangeSymbol(int x, int y, string symbol)
        {
            var item = Get(x, y);
            if (item != null)
                item.IconName = symbol;
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

                if (string.IsNullOrEmpty(item.IconName))
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

            //foreach (var e in this)
            //{
            //    if (e == null)
            //        continue;

            //    if (e.X.Count == 1 && e.Y.Count == 1)
            //    {
            //        if (e.X[0] == x && e.Y[0] == y)
            //            return e;
            //    }
            //    else
            //    {
            //        int xmin = -1;
            //        int xmax = -1;
            //        if (e.X.Count > 1)
            //        {
            //            xmin = Math.Min(e.X[0], e.X[1]);
            //            xmax = Math.Max(e.X[0], e.X[1]);
            //        }
            //        else
            //        {
            //            xmin = e.X[0];
            //            xmax = xmin;
            //        }
                    
            //        int ymin = -1;
            //        int ymax = -1;
            //        if (e.Y.Count > 1)
            //        {
            //            ymin = Math.Min(e.Y[0], e.Y[1]);
            //            ymax = Math.Max(e.Y[0], e.Y[1]);
            //        }
            //        else
            //        {
            //            ymin = e.Y[0];
            //            ymax = ymin;
            //        }

            //        if (x >= xmin && x <= xmax)
            //        {
            //            if (y >= ymin && y <= ymax)
            //                return e;
            //        }
            //    }
            //}

            return null;
        }
    }
}
