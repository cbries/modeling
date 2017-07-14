using System;
using System.Collections.Generic;

namespace TrackPlanParser
{
    public class Track : List<TrackInfo>
    {
        public TrackInfo Get(int x, int y)
        {
            foreach (var e in this)
            {
                if (e == null)
                    continue;

                if (e.X.Count == 1 && e.Y.Count == 1)
                {
                    if (e.X[0] == x && e.Y[0] == y)
                        return e;
                }
                else
                {
                    int xmin = -1;
                    int xmax = -1;
                    if (e.X.Count > 1)
                    {
                        xmin = Math.Min(e.X[0], e.X[1]);
                        xmax = Math.Max(e.X[0], e.X[1]);
                    }
                    else
                    {
                        xmin = e.X[0];
                        xmax = xmin;
                    }
                    
                    int ymin = -1;
                    int ymax = -1;
                    if (e.Y.Count > 1)
                    {
                        ymin = Math.Min(e.Y[0], e.Y[1]);
                        ymax = Math.Max(e.Y[0], e.Y[1]);
                    }
                    else
                    {
                        ymin = e.Y[0];
                        ymax = ymin;
                    }

                    if (x >= xmin && x <= xmax)
                    {
                        if (y >= ymin && y <= ymax)
                            return e;
                    }
                }
            }

            return null;
        }
    }
}
