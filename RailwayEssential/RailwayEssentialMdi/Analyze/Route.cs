using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RailwayEssentialMdi.Analyze
{
    public class Route : List<WayPoint>
    {
        public bool IsBusy { get; set; }
        public DateTime StartBusiness { get; set; }
        public DateTime StopBusiness { get; set; }

        private int LocomotiveObjectId(JObject opts)
        {
            if (opts?["blockCurrentLocomotive"] == null)
                return -1;
            var v = opts["blockCurrentLocomotive"].ToString();
            if (string.IsNullOrEmpty(v))
                return -1;
            if (int.TryParse(v, out var objectId))
                return objectId;
            return -1;
        }

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
