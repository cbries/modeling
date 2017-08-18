using System.Collections.Generic;
using System.Diagnostics;
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

        public int GetRoutes()
        {
            var blks = GetBlocks();

            foreach (var b0 in blks)
            {
                if (b0 == null)
                    continue;

                _branches.Clear();
                _currentWay = "";

                GetAllPath(b0);                
            }

            foreach (var p in _paths)
            {
                if (string.IsNullOrEmpty(p))
                    continue;

                WayPoints wps = new WayPoints(this, p);
                
                if(Routes == null)
                    Routes = new List<WayPoints>();
                Routes.Add(wps);
            }

            if (Routes == null)
                return 0;

            return Routes.Count;
        }
        
        private void GetAllPath(MapItem from)
        {
            if (from == null)
                return;

            var neighbours = from.GetReachableNeighbours();

            foreach (var n in neighbours)
            {
                var nItem = Get(n.X, n.Y);

                StartWalk(nItem, from);
            }

            // check for branches
            if (_branches.Count > 0)
            {
                int max = _branches.Count;
                for (int i = 0; i < max; ++i)
                {
                    var branch = _branches.Pop();
                    if (branch == null)
                        continue;

                    //_currentWay += $"{branch.Item.Identifier} -> ";

                    for (int j = 0; j < branch.Neighbours.Count; ++j)
                    {
                        _currentWay = branch.RecentWay;
                        var nb = branch.Neighbours[j];
                        //branch.Neighbours.Remove(nb);
                        StartWalk(nb, branch.Item);
                    }
                    branch.Neighbours.Clear();
                }
            }
        }

        private void StartWalk(MapItem item, MapItem comingFrom)
        {
            _currentWay += $"{item.Identifier}->";

            Walk(item, comingFrom);
        }

        private void Walk(MapItem item, MapItem comingFrom)
        {
            if (item != null && item.IsBlock)
            {
                _paths.Add(_currentWay);
                _currentWay = "";
                return;
            }

            bool isFromLeft = item.Info.IsLeft(comingFrom.Info);
            bool isFromTop = item.Info.IsUp(comingFrom.Info);
            bool isFromRight = item.Info.IsRight(comingFrom.Info);
            bool isFromBottom = item.Info.IsDown(comingFrom.Info);

            var nbs = item.GetReachableNeighbours(comingFrom.Info);

            if (nbs.Count == 0)
            {
                _currentWay = "";
            }
            else
            {
                if (nbs.Count == 1)
                {
                    var it = nbs[0];
                    var nbsItem = Get(it.X, it.Y);

                    if (item.IsDirection)
                    {
                        bool stopWalk = false;

                        bool nIsLeft = item.Info.IsLeft(it);
                        bool nIsUp = item.Info.IsUp(it);
                        bool nIsRight = item.Info.IsRight(it);
                        bool nIsDown = item.Info.IsDown(it);

                        if (isFromLeft)
                        {
                            if (nIsDown && !item.CanGoFromLeftToBottom()
                                || nIsUp && !item.CanGoFromLeftToTop()
                                || nIsRight && !item.CanGoFromLeftToRight())
                            {
                                stopWalk = true;
                            }
                        }
                        else if (isFromTop)
                        {
                            if (nIsLeft && !item.CanGoFromTopToLeft()
                                || nIsDown && !item.CanGoFromTopToBottom()
                                || nIsRight && !item.CanGoFromTopToRight())
                            {
                                stopWalk = true;
                            }
                        }
                        else if (isFromRight)
                        {
                            if (nIsUp && !item.CanGoFromRightToTop()
                                || nIsDown && !item.CanGoFromRightToBottom()
                                || nIsLeft && !item.CanGoFromRightToLeft())
                            {
                                stopWalk = true;
                            }
                        }
                        else if (isFromBottom)
                        {
                            if (nIsLeft && !item.CanGoFromBottomToLeft()
                                || nIsUp && !item.CanGoFromBottomToTop()
                                || nIsRight && !item.CanGoFromBottomToRight())
                            {
                                stopWalk = true;
                            }
                        }

                        if(stopWalk)
                        {
                            _currentWay = "";
                            return;
                        }
                    }

                    _currentWay += $"{nbsItem.Identifier} -> ";

                    Walk(nbsItem, item);
                }
                else
                {
                    List<int> indecesForRemove = new List<int>();

                    for(int i=0; i < nbs.Count; ++i)
                    {
                        var n = nbs[i];

                        bool nIsLeft = item.Info.IsLeft(n);
                        bool nIsUp = item.Info.IsUp(n);
                        bool nIsRight = item.Info.IsRight(n);
                        bool nIsDown = item.Info.IsDown(n);

                        //string sourceInfo = isFromLeft
                        //    ? "From Left"
                        //    : isFromTop
                        //        ? "From Top"
                        //        : isFromRight
                        //            ? "From Right"
                        //            : isFromBottom
                        //                ? "From Bottom"
                        //                : "Unknown";

                        //string targetInfo = nIsLeft
                        //    ? "Going Left"
                        //    : nIsUp
                        //        ? "Going Up"
                        //        : nIsRight
                        //            ? "Going Right"
                        //            : nIsDown
                        //                ? "Going Down"
                        //                : "Unknown";

                        //Trace.Write($" [{sourceInfo} -> {targetInfo}] ");

                        if (isFromLeft)
                        {
                            if (nIsDown && !item.CanGoFromLeftToBottom()
                                || nIsUp && !item.CanGoFromLeftToTop()
                                || nIsRight && !item.CanGoFromLeftToRight())
                            {
                                indecesForRemove.Add(i);
                            }
                        }
                        else if (isFromTop)
                        {
                            if (nIsLeft && !item.CanGoFromTopToLeft()
                                || nIsDown && !item.CanGoFromTopToBottom()
                                || nIsRight && !item.CanGoFromTopToRight())
                            {
                                indecesForRemove.Add(i);
                            }
                        }
                        else if (isFromRight)
                        {
                            if (nIsUp && !item.CanGoFromRightToTop()
                                || nIsDown && !item.CanGoFromRightToBottom()
                                || nIsLeft && !item.CanGoFromRightToLeft())
                            {
                                indecesForRemove.Add(i);
                            }
                        }
                        else if (isFromBottom)
                        {
                            if (nIsLeft && !item.CanGoFromBottomToLeft()
                                || nIsUp && !item.CanGoFromBottomToTop()
                                || nIsRight && !item.CanGoFromBottomToRight())
                            {
                                indecesForRemove.Add(i);
                            }
                        }
                    }

                    indecesForRemove.Reverse();
                    foreach(var idx in indecesForRemove)
                        nbs.RemoveAt(idx);

                    if (nbs.Count > 1)
                    {
                        var branchInfo = new BranchInfo
                        {
                            Item = item,
                            RecentWay = _currentWay
                        };

                        foreach (var n in nbs)
                        {
                            var nItem = Get(n.X, n.Y);
                            if (nItem != null)
                            {
                                branchInfo.Neighbours.Add(nItem);
                            }
                        }

                        _currentWay = "";

                        _branches.Push(branchInfo);
                    }
                    else if(nbs.Count == 1)
                    {
                        var it = nbs[0];
                        var nbsItem = Get(it.X, it.Y);
                        _currentWay += $"{nbsItem.Identifier} -> ";
                        Walk(nbsItem, item);
                    }
                }
            }
        }

        private class BranchInfo
        {
            public MapItem Item { get; set; }
            public string RecentWay { get; set; }
            public List<MapItem> Neighbours { get; private set; }

            public BranchInfo()
            {
                Neighbours = new List<MapItem>();
            }

            public void NeighbourHasBeenVisited(MapItem neighbour)
            {
                if (neighbour == null)
                    return;
                Neighbours.Remove(neighbour);
            }
        }
        
        private string _currentWay = "";

        private readonly Stack<BranchInfo> _branches = new Stack<BranchInfo>();

        private readonly List<string> _paths = new List<string>();

        public List<WayPoints> Routes { get; private set; }
    }
}
