using System;
using System.Collections.Generic;
using System.Linq;
using RailwayEssentialMdi.ViewModels;
using Theme;
using TrackPlanParser;

namespace RailwayEssentialMdi.Analyze
{
    public class MapItem
    {
        private static int _instanceId = 0;
        private readonly int _localId;

        private TrackInfo _info;

        private readonly Map _ctx;
        private readonly RailwayEssentialModel _model;

        public TrackInfo Info
        {
            get => _info;
            set
            {
                _info = value;

                UpdateOrientation();
            }
        }

        private Entities.TrackEntity TrackEntity => _model?.TrackEntity;
        public int ThemeId => Info?.ThemeId ?? -1;
        private Theme.Theme Theme => _model?.TrackEntity.Theme;
        private int MaxX => TrackEntity.Cfg.DesignerColumns;
        private int MaxY => TrackEntity.Cfg.DesignerRows;
        public string Identifier => $"({Info.X},{Info.Y})";
        public int Idx => _localId;

        private bool _dimensionInitialized;
        private int _orientationIndex;
        private Theme.ThemeItem _themeInfo;
        private ThemeItemDimension _dim;

        private void InitializeDimension()
        {
            if (_dimensionInitialized)
                return;
            _dimensionInitialized = true;
            _orientationIndex = GetOrientation();
            _themeInfo = Theme.Get(ThemeId);
            _dim = _themeInfo.Dimensions[_orientationIndex];
        }

        public int X0
        {
            get
            {
                InitializeDimension();
                return Info.X;
            }
        }

        public int Y0
        {
            get
            {
                InitializeDimension();
                return Info.Y;
            }
        }

        public int X1 => X0 + Width - 1;
        public int Y1 => Y0 + Height - 1;

        public int Width
        {
            get
            {
                InitializeDimension();
                return _dim.X;
            }
        }

        public int Height
        {
            get
            {
                InitializeDimension();
                return _dim.Y;
            }
        }

        private static readonly List<int> TrackIds = new List<int> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
        private static readonly List<int> SwitchIds = new List<int> { 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
        private static readonly List<int> SignalIds = new List<int> { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 };
        private static readonly List<int> BlockIds = new List<int> { 150, 151, 152 };
        private static readonly List<int> SensorIds = new List<int> { 200, 201, 202 };

        public bool IsTrack => TrackIds.Contains(ThemeId);
        public bool IsSwitch => SwitchIds.Contains(ThemeId);
        public bool IsSignal => SignalIds.Contains(ThemeId);
        public bool IsBlock => BlockIds.Contains(ThemeId);
        public bool IsSensor => SensorIds.Contains(ThemeId);

        public MapItem(RailwayEssentialModel model, Map ctx)
        {
            ++_instanceId;

            _localId = _instanceId;

            _ctx = ctx;
            _model = model;
        }

        public bool Left2Right { get; private set; }
        public bool Left2Top { get; private set; }
        public bool Left2Bottom { get; private set; }

        public bool Top2Left { get; private set; }
        public bool Top2Right { get; private set; }
        public bool Top2Bottom { get; private set; }

        public bool Right2Left { get; private set; }
        public bool Right2Top { get; private set; }
        public bool Right2Bottom { get; private set; }

        public bool Bottom2Left { get; private set; }
        public bool Bottom2Right { get; private set; }
        public bool Bottom2Top { get; private set; }

        /// <summary> human-readable information of possible ways </summary>
        public string DirectionInfo
        {
            get
            {
                string m = "";

                if (Left2Right) m += "Left2Right, ";
                if (Left2Top) m += "Left2Top, ";
                if (Left2Bottom) m += "Left2Bottom, ";

                if (Top2Left) m += "Top2Left, ";
                if (Top2Right) m += "Top2Right, ";
                if (Top2Bottom) m += "Top2Bottom, ";

                if (Right2Left) m += "Right2Left, ";
                if (Right2Top) m += "Right2Top, ";
                if (Right2Bottom) m += "Right2Bottom, ";

                if (Bottom2Left) m += "Bottom2Left, ";
                if (Bottom2Right) m += "Bottom2Right, ";
                if (Bottom2Top) m += "Bottom2Top";

                return m.Trim().TrimEnd(',');
            }
        }
        
        private void UpdateOrientation()
        {
            ThemeItemRoute e = GetWays();

            List<string> parts = new List<string>();

            if (IsTrack || IsSignal || IsBlock || IsSensor)
            {
                parts = e.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (parts.Count <= 1
                    && !parts[0].EndsWith("!", StringComparison.OrdinalIgnoreCase)
                    && !parts[0].EndsWith("+", StringComparison.OrdinalIgnoreCase))
                {
                    parts.Add(parts[0].Reverse());
                }
            }
            else if (IsSwitch)
            {
                parts = e.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            foreach (var p in parts)
            {
                if (p.Length == 2)
                {
                    if (p[0] == 'A' && p[1] == 'C')
                        Left2Right = true;
                    if (p[0] == 'A' && p[1] == 'B')
                        Left2Top = true;
                    if (p[0] == 'A' && p[1] == 'D')
                        Left2Bottom = true;

                    if (p[0] == 'B' && p[1] == 'A')
                        Top2Left = true;
                    if (p[0] == 'B' && p[1] == 'C')
                        Top2Right = true;
                    if (p[0] == 'B' && p[1] == 'D')
                        Top2Bottom = true;

                    if (p[0] == 'C' && p[1] == 'A')
                        Right2Left = true;
                    if (p[0] == 'C' && p[1] == 'B')
                        Right2Top = true;
                    if (p[0] == 'C' && p[1] == 'D')
                        Right2Bottom = true;

                    if (p[0] == 'D' && p[1] == 'A')
                        Bottom2Left = true;
                    if (p[0] == 'D' && p[1] == 'B')
                        Bottom2Top = true;
                    if (p[0] == 'D' && p[1] == 'C')
                        Bottom2Right = true;
                }
            }
        }

        public bool CanMoveUp { get; private set; }
        public bool CanMoveDown { get; private set; }
        public bool CanMoveLeft { get; private set; }
        public bool CanMoveRight { get; private set; }

        #region check exit states

        public bool IsLeftExit
        {
            get
            {
                if (Top2Left) return true;
                if (Right2Left) return true;
                if (Bottom2Left) return true;
                return false;
            }
        }

        public bool IsTopExit
        {
            get
            {
                if (Left2Top) return true;
                if (Right2Top) return true;
                if (Bottom2Top) return true;
                return false;
            }
        }

        public bool IsRightExit
        {
            get
            {
                if (Left2Right) return true;
                if (Top2Right) return true;
                if (Bottom2Right) return true;
                return false;
            }
        }

        public bool IsBottomExit
        {
            get
            {
                if (Left2Bottom) return true;
                if (Top2Bottom) return true;
                if (Right2Bottom) return true;
                return false;
            }
        }

        #endregion

        #region check entrance states

        public bool IsLeftEntrance
        {
            get
            {
                if (Left2Bottom) return true;
                if (Left2Right) return true;
                if (Left2Top) return true;
                return false;
            }
        }

        public bool IsTopEntrance
        {
            get
            {
                if (Top2Bottom) return true;
                if (Top2Left) return true;
                if (Top2Right) return true;
                return false;
            }
        }

        public bool IsRightEntrance
        {
            get
            {
                if (Right2Left) return true;
                if (Right2Top) return true;
                if (Right2Bottom) return true;
                return false;
            }
        }

        public bool IsBottomEntrance
        {
            get
            {
                if (Bottom2Top) return true;
                if (Bottom2Left) return true;
                if (Bottom2Right) return true;
                return false;
            }
        }

        #endregion

        public void UpdateMovement()
        {
            var neighbours = GetNeighbours();

            if (neighbours.Count <= 0)
            {
                CanMoveDown = false;
                CanMoveLeft = false;
                CanMoveRight = false;
                CanMoveUp = false;

                return;
            }

            foreach (var n in neighbours)
            {
                if (n == null || n.ThemeId == -1)
                    continue;

                MapItem nItem = _ctx.Get(n.Info.X, n.Info.Y);

                if (Info.IsLeft(n.Info))
                    CanMoveLeft = IsLeftExit && nItem.IsRightEntrance;
                else if (Info.IsRight(n.Info))
                    CanMoveRight = IsRightExit && nItem.IsLeftEntrance;
                else if (Info.IsUp(n.Info))
                    CanMoveUp = IsTopExit && nItem.IsBottomEntrance;
                else if (Info.IsDown(n.Info))
                    CanMoveDown = IsBottomExit && nItem.IsTopEntrance;
            }
        }

        private List<MapItem> GetNeighbours(TrackInfo ignore = null)
        {
            if (Info == null)
                return null;

            List<MapItem> neighbours = new List<MapItem>();

            var x = Info.X;
            var y = Info.Y;

            var width = Width;
            var height = Height;

            List<Coord> indeces = new List<Coord>();

            #region edges

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

            #endregion

            foreach (var e in indeces)
            {
                if (ignore?.X == e.X && ignore.Y == e.Y)
                    continue;

                var item = _ctx.Get(e.X, e.Y);
                if (item != null)
                    neighbours.Add(item);
            }

            return neighbours;
        }

        public List<TrackInfo> GetReachableNeighbours()
        {
            List<TrackInfo> neighbours = new List<TrackInfo>();

            const int max = 10;
            int maxCounter = 0;

            var x = Info.X;
            var y = Info.Y;

            var width = Width;
            var height = Height;

            if (CanMoveDown)
            {
                var yy = y + height;

                var item = TrackEntity.Track.Get(x, yy);
                if (item != null)
                    neighbours.Add(item);
            }
            if (CanMoveUp)
            {
                var yy = y - 1;

                var item = TrackEntity.Track.Get(x, yy);
                maxCounter = 0;
                while (item == null && maxCounter < max)
                {
                    --yy;

                    item = TrackEntity.Track.Get(x, yy);
                }

                if (item != null)
                    neighbours.Add(item);
            }
            if (CanMoveLeft)
            {
                var xx = x - 1;

                var item = TrackEntity.Track.Get(xx, y);
                maxCounter = 0;
                while (item == null && maxCounter < max)
                {
                    --xx;

                    item = TrackEntity.Track.Get(xx, y);
                }

                if (item != null)
                    neighbours.Add(item);
            }
            if (CanMoveRight)
            {
                var xx = x + width;

                var item = TrackEntity.Track.Get(xx, y);
                if (item != null)
                    neighbours.Add(item);
            }
            return neighbours;
        }

        public List<int> GetReachableNeighbourIds()
        {
            List<int> indeces = new List<int>();
            var neighbours = GetReachableNeighbours();
            foreach (var n in neighbours)
            {
                MapItem nItem = _ctx.Get(n.X, n.Y);
                indeces.Add(nItem.Idx);
            }
            return indeces;
        }

        public int GetOrientation()
        {
            if (Info == null)
                return 0;
            if (string.IsNullOrEmpty(Info.Orientation))
                return 0;
            if (Info.Orientation.Equals("rot0", StringComparison.OrdinalIgnoreCase))
                return 0;
            if (Info.Orientation.Equals("rot90", StringComparison.OrdinalIgnoreCase))
                return 1;
            if (Info.Orientation.Equals("rot180", StringComparison.OrdinalIgnoreCase))
                return 2;
            if (Info.Orientation.Equals("rot-90", StringComparison.OrdinalIgnoreCase))
                return 3;
            return 0;
        }

        /// <summary>
        ///     "AB"        -> Straight
        ///     "CA,CD"     -> Switch
        ///     "CA,CB,CD"  -> Threeway
        /// </summary>
        /// <returns></returns>
        public ThemeItemRoute GetWays()
        {
            var themeInfo = Theme.Get(ThemeId);
            if (themeInfo == null)
                return null;
            int index = GetOrientation();
            return themeInfo.GetRoute(index);
        }
    }
}
