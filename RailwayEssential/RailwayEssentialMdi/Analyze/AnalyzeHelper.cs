using System;
using TrackPlanParser;

namespace RailwayEssentialMdi.Analyze
{
    public static class AnalyzeHelper
    {
        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static bool IsLeft(this TrackInfo item, TrackInfo neighbourItem)
        {
            if (neighbourItem == null)
                return false;
            if (neighbourItem.X - 1 < item.X && neighbourItem.Y == item.Y)
                return true;
            return false;
        }

        public static bool IsRight(this TrackInfo item, TrackInfo neighbourItem)
        {
            if (neighbourItem == null)
                return false;
            if (neighbourItem.X + 1 > item.X && neighbourItem.Y == item.Y)
                return true;
            return false;
        }

        public static bool IsUp(this TrackInfo item, TrackInfo neighbourItem)
        {
            if (neighbourItem == null)
                return false;
            if (neighbourItem.Y - 1 < item.Y && neighbourItem.X == item.X)
                return true;
            return false;
        }

        public static bool IsDown(this TrackInfo item, TrackInfo neighbourItem)
        {
            if (neighbourItem == null)
                return false;
            if (neighbourItem.Y + 1 > item.Y && neighbourItem.X == item.X)
                return true;
            return false;
        }
    }
}