using System.Collections.Generic;

namespace RailwayEssentialCore
{
    public class Globals
    {
        public enum ThemeIdType
        {
            Track, Switch, Signal, Block, Sensor, Direction, Connector
        }

        public static readonly List<int> TrackIds = new List<int> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
        public static readonly List<int> SwitchIds = new List<int> { 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
        public static readonly List<int> SignalIds = new List<int> { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 };
        public static readonly List<int> BlockIds = new List<int> { 150, 151, 152 };
        public static readonly List<int> SensorIds = new List<int> { 200, 201, 202 };
        public static readonly List<int> DirectionIds = new List<int> { 14 };
        public static readonly List<int> ConnectorIds = new List<int> { 17, 18, 19 };

        public static ThemeIdType GetThemeType(int themeId)
        {
            if(DirectionIds.Contains(themeId)) return ThemeIdType.Direction;
            if(ConnectorIds.Contains(themeId)) return ThemeIdType.Connector;
            if(SensorIds.Contains(themeId)) return ThemeIdType.Sensor;
            if(BlockIds.Contains(themeId)) return ThemeIdType.Block;
            if(SignalIds.Contains(themeId)) return ThemeIdType.Signal;
            if(SwitchIds.Contains(themeId)) return ThemeIdType.Switch;
            if(TrackIds.Contains(themeId)) return ThemeIdType.Track;
            return ThemeIdType.Track;
        }

        public static string GetThemeTypeName(int themeId)
        {
            var type = GetThemeType(themeId);
            switch (type)
            {
                    case ThemeIdType.Block: return "Block";
                    case ThemeIdType.Connector: return "Connector";
                    case ThemeIdType.Direction: return "Direction";
                    case ThemeIdType.Sensor: return "Sensor";
                    case ThemeIdType.Signal: return "Signal";
                    case ThemeIdType.Switch: return "Switch";
                    case ThemeIdType.Track: return "Track";
            }
            return "Track";
        }
    }
}
