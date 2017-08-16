using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RailwayEssentialMdi.Analyze.BFS;
using RailwayEssentialMdi.ViewModels;

namespace RailwayEssentialMdi.Analyze
{
    public class Map
    {
        private readonly RailwayEssentialModel _model;

        public List<MapItem> Items { get; private set; }

        public Map(RailwayEssentialModel model)
        {
            _model = model;

            Items = new List<MapItem>();
        }

        public MapItem Get(int x, int y)
        {
            foreach (var e in Items)
            {
                if (e == null)
                    continue;

                if (e.X0 == x && e.Y0 == y)
                    return e;

                if (e.X1 == x && e.Y1 == y)
                    return e;
            }

            return null;
        }

        public void Build()
        {
            if (_model?.TrackEntity == null)
                return;

            var o = _model?.TrackEntity;

            var maxX = o.Cfg.DesignerColumns;
            var maxY = o.Cfg.DesignerRows;

            for (int x = 0; x <= maxX; ++x)
            {
                for (int y = 0; y <= maxY; ++y)
                {
                    var it = o.Track.Get(x, y);

                    if (it == null)
                        continue;

                    if (it.ThemeId == -1)
                        continue;

                    var mapItem = new MapItem(_model, this)
                    {
                        Info = it
                    };

                    Items.Add(mapItem);
                }
            }

            foreach (var item in Items)
                item?.UpdateMovement();
        }

        private List<MapItem> GetBlocks()
        {
            List<MapItem> blocks = new List<MapItem>();

            foreach (var e in Items)
            {
                if(e == null)
                    continue;

                if (e.IsBlock)
                    blocks.Add(e);
            }

            return blocks;
        }

        public Array GetEdges()
        {
            var vertices = Items.Where(x => x.Idx != -1).ToList().OrderBy(x => x.Idx).ToList();
            var idxs = new List<int>();

            foreach (var v in vertices)
                idxs.Add(v.Idx);

            List<Tuple<int,int>> edges = new List<Tuple<int, int>>();

            foreach (var it in Items)
            {
                if (it == null)
                    continue;

                var localIdx = it.Idx;
                var neighbourIdxs = it.GetReachableNeighbourIds();

                foreach (var n in neighbourIdxs)
                {
                    edges.Add(Tuple.Create(localIdx, n));
                    edges.Add(Tuple.Create(n, localIdx));
                }
            }

            var graph = new Graph<int>(idxs, edges);
            var algorithms = new Algorithms();

            var blks = GetBlocks();

            foreach (var b0 in blks)
            {
                if (b0 == null)
                    continue;

                var startVertex = b0.Idx;
                var shortestPath = algorithms.ShortestPathFunction(graph, startVertex);

                foreach (var b1 in blks)
                {
                    if (b1 == null || b1.Equals(b0))
                        continue;

                    var pathIdx = shortestPath(b1.Idx).ToList();

                    string m = "";

                    foreach (var p in pathIdx)
                    {
                        var item = Items.Where(x => x.Idx == p).ToList();
                        if (item.Count > 0)
                        {
                            var coord = $"[{item[0].X0}, {item[0].Y0} ; {item[0].X1}, {item[0].Y1}]";

                            m += $"{p} {coord} -> ";
                        }
                    }

                    m += " (!!) ";

                    Trace.WriteLine(string.Format("Path to {0,2}: {1}", b1.Idx, m));
                }
            }

            return null;
        }
    }
}
