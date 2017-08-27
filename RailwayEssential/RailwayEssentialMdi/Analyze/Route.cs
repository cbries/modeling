using System.Collections.Generic;

namespace RailwayEssentialMdi.Analyze
{
    public class Route : List<WayPoint>
    {
        public static bool Cross(Route r0, Route r1)
        {
            foreach (var runner0 in r0)
            {
                foreach (var runner1 in r1)
                {
                    if (runner0.X == runner1.X)
                    {
                        if (runner0.Y == runner1.Y)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
