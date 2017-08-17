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
                if (e == null)
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

            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();

            foreach (var it in Items)
            {
                if (it == null)
                    continue;

                var item = it.Info;
                if (item == null)
                    continue;

                var localIdx = it.Idx;
                var neighbourIdxs = it.GetReachableNeighbourIds();
                foreach (var n in neighbourIdxs)
                    edges.Add(Tuple.Create(localIdx, n));
            }

            var blks = GetBlocks();

            foreach (var b0 in blks)
            {
                if (b0 == null)
                    continue;

                foreach (var b1 in blks)
                {
                    if (b1 == null)
                        continue;
                    if (b1.Equals(b0))
                        continue;

                    var paths = GetAllPathBetween(b0, b1);
                }
            }

            foreach (var item in Items)
            {
                if (item == null)
                    continue;

                var ways = new MapItem.WayInfo(item);

                Trace.WriteLine($"{item.Info} Ways: {ways}");
            }

            //var graph = new Graph<int>(idxs, edges);
            //var algorithms = new Algorithms();
            //var blks = GetBlocks();
            //foreach (var b0 in blks)
            //{
            //    if (b0 == null)
            //        continue;
            //    var b0NeighboursIdxs = b0.GetReachableNeighbourIds();
            //    foreach (var b0Idx in b0NeighboursIdxs)
            //    {
            //        var startVertex = b0Idx;
            //        var shortestPath = algorithms.ShortestPathFunction(graph, startVertex);
            //        foreach (var b1 in blks)
            //        {
            //            if (b1 == null || b1.Equals(b0))
            //                continue;
            //            var pathIdx = shortestPath(b1.Idx).ToList();
            //            string m = "";
            //            foreach (var p in pathIdx)
            //            {
            //                var item = Items.Where(x => x.Idx == p).ToList();
            //                if (item.Count > 0)
            //                {
            //                    var coord = $"({item[0].X0},{item[0].Y0})";
            //                    m += $"{coord} -> ";
            //                }
            //            }
            //            m += " (!!) ";
            //            var fromInfo = $"{b0.X0},{b0.Y1}";
            //            var toInfo = $"{b1.X0},{b1.Y1}";
            //            Trace.WriteLine(string.Format("{0} -> {1}: {2}", fromInfo, toInfo, m));
            //        }
            //    }
            //}

            return null;
        }

        private Dictionary<int, List<int>> GetAllPathBetween(MapItem from, MapItem to)
        {
            if (from == null || to == null)
                return null;

            Trace.WriteLine($"Find path: {from.Identifier} -> {to.Identifier}");

            var paths = new Dictionary<int, List<int>>();

            var neighbours = from.GetReachableNeighbours();
            foreach (var n in neighbours)
            {
                var nItem = Get(n.X, n.Y);

                Walk(nItem, from);
            }

            return paths;
        }

        private void Walk(MapItem item, MapItem comingFrom)
        {
            
        }
    }
}
