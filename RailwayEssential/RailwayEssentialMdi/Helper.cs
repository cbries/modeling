using System;
using TrackPlanParser;

namespace RailwayEssentialMdi
{
    internal class Helper
    {
        public static int GetOrientation(TrackInfo info)
        {
            if (info == null)
                return 0;
            if (string.IsNullOrEmpty(info.Orientation))
                return 0;
            if (info.Orientation.Equals("rot0", StringComparison.OrdinalIgnoreCase))
                return 0;
            if (info.Orientation.Equals("rot90", StringComparison.OrdinalIgnoreCase))
                return 1;
            if (info.Orientation.Equals("rot180", StringComparison.OrdinalIgnoreCase))
                return 2;
            if (info.Orientation.Equals("rot-90", StringComparison.OrdinalIgnoreCase))
                return 3;
            return 0;
        }
    }
}
