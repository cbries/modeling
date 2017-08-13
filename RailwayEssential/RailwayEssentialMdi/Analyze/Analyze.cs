using System;
using System.Collections.Generic;
using System.Diagnostics;
using RailwayEssentialMdi.ViewModels;
using Theme;
using TrackPlanParser;

namespace RailwayEssentialMdi.Analyze
{
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

                List<int> blockIds = new List<int> {150, 151};

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
            {
                var waypoints = GetWaypoints(b);
                if (waypoints != null && waypoints.Count > 0)
                {
                    Trace.WriteLine("Waypoints: " + waypoints.Count);
                }
            }

            AnalyzeResult res = new AnalyzeResult();

            return res;
        }

        private int GetOrientation(TrackInfo trackInfo)
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

        private ThemeItemRoute GetThemeWays(TrackInfo trackInfo)
        {
            if (trackInfo == null)
                return null;
            var themeId = trackInfo.ThemeId;
            if (themeId < 0)
                return null;
            if (Theme == null)
                return null;
            var themeInfo = Theme.Get(trackInfo.ThemeId);
            if (themeInfo == null)
                return null;

            int index = GetOrientation(trackInfo);

            return themeInfo.GetRoute(index);
        }

        private List<Waypoints> GetWaypoints(TrackInfo block)
        {
            var neighbours = GetNeighbours(block);

            return null;
        }

        private List<TrackInfo> GetNeighbours(TrackInfo trackInfo)
        {
            if (trackInfo == null)
                return null;

            List<TrackInfo> neighbours = new List<TrackInfo>();

            var x = trackInfo.X;
            var y = trackInfo.Y;
            int orientationIndex = GetOrientation(trackInfo);

            // dimension required
            var themeInfo = Theme.Get(trackInfo.ThemeId);
            var dim = themeInfo.Dimensions[orientationIndex];
            var width = dim.X;
            var height = dim.Y;

            Dictionary<int, int> indeces = new Dictionary<int, int>();

            if (width == 1 && height == 1)
            {
                if(x - 1 > 0)
                    indeces.Add(x - 1, y);
                if(x + 1 <= MaxX)
                    indeces.Add(x + 1, y);
                if(y - 1 > 0)
                    indeces.Add(x, y - 1);
                if(y + 1 <= MaxY)
                    indeces.Add(x, y + 1);
            }
            else
            {
                // top edge
                var startX = x;
                var startY = y - 1;
                if (startY > 0)
                {
                    for (int col = 0; col < width; ++col)
                        indeces.Add(startX + col, startY);
                }

                // bottom edge
                startX = x;
                startY = y + height;
                if (startY <= MaxY)
                {
                    for (int col = 0; col < width; ++col)
                        indeces.Add(startX + col, startY);
                }

                // left edge
                startX = x - 1;
                if (startX > 0)
                {
                    for (int row = 0; row < height; ++row)
                        indeces.Add(startX, startY + row);
                }

                // right edge
                startX = x + width;
                if (startX <= MaxX)
                {
                    for (int row = 0; row < height; ++row)
                        indeces.Add(startX, startY + row);
                }
            }

            foreach (var k in indeces.Keys)
            {
                var v = indeces[k];

                var item = TrackEntity.Track.Get(k, v);
                if (item != null)
                    neighbours.Add(item);
            }

            return neighbours;
        }
    }
}
