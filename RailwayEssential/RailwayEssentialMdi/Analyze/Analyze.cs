using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RailwayEssentialMdi.ViewModels;
using Theme;
using TrackPlanParser;

namespace RailwayEssentialMdi.Analyze
{
    public enum AnalyzeDirection
    {
        Left,
        Right,
        Up,
        Down,
        Unknwon
    };

    public class AnalyzeUtils
    {
        public static int GetOrientation(TrackInfo trackInfo)
        {
            if (trackInfo == null)
                return 0;
            if (string.IsNullOrEmpty(trackInfo.Orientation))
                return 0;
            if (trackInfo.Orientation.Equals("rot0", StringComparison.OrdinalIgnoreCase))
                return 0;
            if (trackInfo.Orientation.Equals("rot90", StringComparison.OrdinalIgnoreCase))
                return 1;
            if (trackInfo.Orientation.Equals("rot180", StringComparison.OrdinalIgnoreCase))
                return 2;
            if (trackInfo.Orientation.Equals("rot-90", StringComparison.OrdinalIgnoreCase))
                return 3;
            return 0;
        }

        public static ThemeItemRoute GetThemeWays(TrackInfo trackInfo, Theme.Theme theme)
        {
            if (trackInfo == null)
                return null;
            var themeId = trackInfo.ThemeId;
            if (themeId < 0)
                return null;
            if (theme == null)
                return null;
            var themeInfo = theme.Get(trackInfo.ThemeId);
            if (themeInfo == null)
                return null;

            int index = GetOrientation(trackInfo);

            return themeInfo.GetRoute(index);
        }

        public static List<string> Ways(ThemeItemRoute ways)
        {
            // "AB"
            // "CA,CD"
            // "CA,CB,CD"

            var parts = ways.Value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            return parts.ToList();
        }
    }

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

        public static bool IsMoveAllowed(this TrackInfo fromItem, TrackInfo toItem, Theme.Theme theme)
        {
            var themeWayFrom = AnalyzeUtils.GetThemeWays(fromItem, theme);
            var waysFrom = AnalyzeUtils.Ways(themeWayFrom);
            if(waysFrom.Count == 1)
                waysFrom.Add(waysFrom[0].Reverse());

            var themeWayTo = AnalyzeUtils.GetThemeWays(toItem, theme);
            var waysTo = AnalyzeUtils.Ways(themeWayTo);
            if(waysTo.Count == 1 && !waysTo[0].EndsWith("!", StringComparison.OrdinalIgnoreCase))
                waysTo.Add(waysTo[0].Reverse());

            var dir = fromItem.Direction(toItem);

            switch (dir)
            {
                    case AnalyzeDirection.Down:
                        foreach (var from in waysFrom)
                        {
                            if (from.EndsWith("D", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (var to in waysTo)
                                {
                                    if (to.StartsWith("B", StringComparison.OrdinalIgnoreCase))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case AnalyzeDirection.Up:
                        foreach (var from in waysFrom)
                        {
                            if (from.EndsWith("B", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (var to in waysTo)
                                {
                                    if (to.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case AnalyzeDirection.Left:
                        foreach (var from in waysFrom)
                        {
                            if (from.EndsWith("A", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (var to in waysTo)
                                {
                                    if (to.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case AnalyzeDirection.Right:
                        foreach (var from in waysFrom)
                        {
                            if (from.EndsWith("C", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (var to in waysTo)
                                {
                                    if (to.StartsWith("A", StringComparison.OrdinalIgnoreCase))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
            }

            return false;
        }

        public static AnalyzeDirection Direction(this TrackInfo item, TrackInfo neighbourItem)
        {
            if (item.IsLeft(neighbourItem))
                return AnalyzeDirection.Left;
            if (item.IsRight(neighbourItem))
                return AnalyzeDirection.Right;
            if (item.IsUp(neighbourItem))
                return AnalyzeDirection.Up;
            if (item.IsDown(neighbourItem))
                return AnalyzeDirection.Down;
            return AnalyzeDirection.Unknwon;
        }
    }

    public class Analyze
    {
        private readonly RailwayEssentialModel _model;
        private Entities.TrackEntity TrackEntity => _model?.TrackEntity;
        private Theme.Theme Theme => _model?.TrackEntity.Theme;

        private int MaxX => TrackEntity.Cfg.DesignerColumns;
        private int MaxY => TrackEntity.Cfg.DesignerRows;

        public Analyze(RailwayEssentialModel model)
        {
            _model = model;
        }

        private List<TrackInfo> Blocks
        {
            get
            {
                if (_model?.TrackEntity == null)
                    return null;

                List<TrackInfo> blocks = new List<TrackInfo>();

                var o = _model?.TrackEntity;

                var maxX = o.Cfg.DesignerColumns;
                var maxY = o.Cfg.DesignerRows;

                List<int> blockIds = new List<int> {150, 151, 152};

                for (int x = 0; x <= maxX; ++x)
                {
                    for (int y = 0; y <= maxY; ++y)
                    {
                        var item = o.Track.Get(x, y);

                        if (item != null)
                        {
                            if(blockIds.Contains(item.ThemeId))
                                blocks.Add(item);
                        }
                    }
                }

                return blocks;
            }
        }

        public AnalyzeResult Execute()
        {
            if (TrackEntity == null || Theme == null)
                return null;

            var blocks = Blocks;

            foreach (var b in blocks)
                GetWaypointsStart(b);

            foreach(var r in Routes)
                Trace.WriteLine(r);

            AnalyzeResult res = new AnalyzeResult();

            return res;
        }

        private int Index => Indeces.Last();
        private Stack<int> Indeces = new Stack<int>();
        private List<string> Routes = new List<string>();
        private string R {
            get => Routes[Index];
            set { Routes[Index] = value; }
        }

        private int GetWaypointsStart(TrackInfo block)
        {
            Routes.Add("R:");
            Indeces.Push(Routes.Count - 1);
            GetWaypoints(block);
            List<int> indecesToRemove = new List<int>();
            for (int i = 0; i < Routes.Count; ++i)
            {
                var r = Routes[i];
                if (r.Length <= 2)
                    indecesToRemove.Add(i);
            }
            indecesToRemove.Reverse();
            foreach(var idx in indecesToRemove)
                Routes.RemoveAt(idx);

            return 0;
        }

        private void GetWaypoints(TrackInfo item, TrackInfo previousItem = null)
        {
            var neighbours = GetNeighbours(item, previousItem);

            if (neighbours.Count == 0)
            {
                R += " (!!) ";

                var m = R;

                Indeces.Pop();

                if (Indeces.Count > 0)
                {
                    Routes.Add(m);
                    Indeces.Push(Routes.Count - 1);
                }

                if (Indeces.Count == 0)
                {
                    Routes.Add("R:");
                    Indeces.Push(Routes.Count - 1);
                }

                return;
            }

            for (int i = 0; i < neighbours.Count; ++i)
            {
                var neighbour = neighbours[i];
                if (neighbour == null)
                    continue;

                if (!string.IsNullOrEmpty(R))
                {
                    string pattern = $"({neighbour.X}, {neighbour.Y})";

                    if (R.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        R += " (!!) ";

                        return;
                    }
                }

                if (neighbours.Count > 1)
                {
                    string pattern = $"({item.X}, {item.Y})";
                    if (Routes.Count >= 2)
                    {
                        string m = Routes[Routes.Count - 2];
                        int index = m.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
                        if (index != -1)
                        {
                            R += m.Substring(0, index + pattern.Length) + " -> ";                            
                        }
                    }
                }

                R += $"({neighbour.X}, {neighbour.Y}) -> ";

                GetWaypoints(neighbour, item);
            }
        }

        private List<TrackInfo> GetNeighbours(TrackInfo trackInfo, TrackInfo ignore = null)
        {
            if (trackInfo == null)
                return null;

            List<TrackInfo> neighbours = new List<TrackInfo>();

            var x = trackInfo.X;
            var y = trackInfo.Y;
            int orientationIndex = AnalyzeUtils.GetOrientation(trackInfo);

            // dimension required
            var themeInfo = Theme.Get(trackInfo.ThemeId);
            var dim = themeInfo.Dimensions[orientationIndex];
            var width = dim.X;
            var height = dim.Y;

            List<Coord> indeces = new List<Coord>();

            if (width == 1 && height == 1)
            {
                if(x - 1 > 0)
                    indeces.Add(new Coord(x -1, y));
                if(x + 1 <= MaxX)
                    indeces.Add(new Coord(x + 1, y));
                if(y - 1 > 0)
                    indeces.Add(new Coord(x, y - 1));
                if(y + 1 <= MaxY)
                    indeces.Add(new Coord(x, y + 1));
            }
            else
            {
                // top edge
                var startX = x;
                var startY = y - 1;
                if (startY > 0)
                {
                    for (int col = 0; col < width; ++col)
                        indeces.Add(new Coord(startX + col, startY));
                }

                // bottom edge
                startX = x;
                startY = y + height;
                if (startY <= MaxY)
                {
                    for (int col = 0; col < width; ++col)
                        indeces.Add(new Coord(startX + col, startY));
                }

                // left edge
                startY = y;
                startX = x - 1;
                if (startX > 0)
                {
                    for (int row = 0; row < height; ++row)
                        indeces.Add(new Coord(startX, startY + row));
                }

                // right edge
                startY = y;
                startX = x + width;
                if (startX <= MaxX)
                {
                    for (int row = 0; row < height; ++row)
                        indeces.Add(new Coord(startX, startY + row));
                }
            }

            foreach (var e in indeces)
            {
                if (ignore?.X == e.X && ignore.Y == e.Y)
                    continue;
                
                var item = TrackEntity.Track.Get(e.X, e.Y);
                if (item != null)
                {
                    var isMoveAllowed = trackInfo.IsMoveAllowed(item, Theme);
                    if(isMoveAllowed)
                        neighbours.Add(item);
                }
            }

            return neighbours;
        }
    }
}
