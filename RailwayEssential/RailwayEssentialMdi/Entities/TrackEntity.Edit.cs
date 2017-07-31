
using System.Collections.Generic;

namespace RailwayEssentialMdi.Entities
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using TrackWeaver;
    using TrackInformation;

    public partial class TrackEntity
    {
        private const int TabIndexS88 = 0;
        private const int TabIndexSwitch = 1;

        private S88 _itemS88Selection;
        private TrackInformation.Switch _itemSwitchSelection;
        private int _itemsS88SelectionPin;

        public S88 ItemsS88Selection
        {
            get => _itemS88Selection;
            set
            {
                _itemS88Selection = value;
                if (_itemS88Selection == null)
                    ItemsS88SelectionPin = -1;
                RaisePropertyChanged("ItemsS88Selection");
            }
        }
        public TrackInformation.Switch ItemsSwitchSelection
        {
            get => _itemSwitchSelection;
            set
            {
                _itemSwitchSelection = value;
                RaisePropertyChanged("ItemsSwitchSelection");
            }
        }
        public int ItemsS88SelectionPin
        {
            get => _itemsS88SelectionPin;
            set
            {
                _itemsS88SelectionPin = value;
                RaisePropertyChanged("ItemsS88SelectionPin");
            }
        }
        public int SelectionX { get; private set; }
        public int SelectionY { get; private set; }
        public bool SelectionXYvisible { get; private set; }

        private int _selectionTabIndex = TabIndexS88;

        public int SelectionTabIndex
        {
            get => _selectionTabIndex;
            set
            {
                _selectionTabIndex = value;
                RaisePropertyChanged("SelectionTabIndex");
            }
        }

        private bool _showObjectEdit;

        public bool ShowObjectEdit
        {
            get => _showObjectEdit;
            set
            {
                _showObjectEdit = value;

                RaisePropertyChanged("ShowObjectEdit");
            }
        }

        private ObservableCollection<S88> _itemsS88 = new ObservableCollection<S88>();
        private ObservableCollection<TrackInformation.Switch> _itemsSwitch = new ObservableCollection<TrackInformation.Switch>();

        public ObservableCollection<S88> ItemsS88
        {
            get => _itemsS88;
            set
            {
                _itemsS88 = value;
                RaisePropertyChanged("ItemsS88");
            }
        }

        public ObservableCollection<TrackInformation.Switch> ItemsSwitch
        {
            get => _itemsSwitch;
            set
            {
                _itemsSwitch = value;
                RaisePropertyChanged("ItemsSwitch");
            }
        }

        internal void ApplyAssignment()
        {
            var weaver = _dispatcher.Weaver;
            if (weaver == null)
                return;

            var track = Track;
            var trackInfo = track.Get(SelectionX, SelectionY);

            if (trackInfo == null)
                return;

            var m = _dispatcher.Model as ViewModels.RailwayEssentialModel;
            if (m == null)
                return;

            var prj = m.Project;

            var weaveFilepath = Path.Combine(prj.Dirpath, prj.Track.Weave);
            TrackWeaveItems weaverItems = new TrackWeaveItems();
            if (!weaverItems.Load(weaveFilepath))
                return;

            var x = SelectionX;
            var y = SelectionY;

            TrackWeaveItem item = null;

            foreach (var e in weaverItems.Items)
            {
                if (e == null)
                    continue;


                if (e.VisuX == x && e.VisuY == y)
                {
                    item = e;

                    break;
                }
            }

            if (item != null)
                weaverItems.Items.Remove(item);

            if (item == null)
            {
                item = new TrackWeaveItem();

                bool addItemCheck = ItemsS88Selection != null || ItemsSwitchSelection != null;

                if (addItemCheck)
                    weaverItems.Items.Add(item);
            }

            item.VisuX = x;
            item.VisuY = y;

            if (ItemsS88Selection != null)
            {
                item.Type = WeaveItemT.S88;
                item.ObjectId = ItemsS88Selection.ObjectId;
                item.Pin = ItemsS88SelectionPin;
            }

            if (ItemsSwitchSelection != null)
            {
                item.Type = WeaveItemT.Switch;
                item.ObjectId = ItemsSwitchSelection.ObjectId;
            }

            bool res = weaverItems.Save();
            if (!res)
            {
                Trace.WriteLine("<Error> Storing of weave file failed.");
            }
            else
            {
                // reload weave

                _dispatcher.InitializeWeaving(Track, weaveFilepath);
            }
        }

        private TrackInformationCore.IItem GetObject(int x, int y)
        {
            var track = Track;
            var trackInfo = track.Get(x, y);

            if (trackInfo == null)
                return null;

            var weaver = _dispatcher.Weaver;
            if (weaver != null)
            {
                var ws = weaver.WovenSeam;
                if (ws != null)
                {
                    foreach (var seam in ws)
                    {
                        if (seam == null)
                            continue;

                        if (seam.TrackObjects.ContainsKey(trackInfo))
                            return seam.ObjectItem;
                    }
                }
            }

            return null;
        }

        private TrackWeaveItem GetWeaveItem(int x, int y)
        {
            var m = _dispatcher.Model as ViewModels.RailwayEssentialModel;
            if (m == null)
                return null;

            var prj = m.Project;

            var weaveFilepath = Path.Combine(prj.Dirpath, prj.Track.Weave);
            TrackWeaveItems weaverItems = new TrackWeaveItems();
            if (!weaverItems.Load(weaveFilepath))
                return null;

            foreach (var e in weaverItems.Items)
            {
                if (e?.VisuX == x && e.VisuY == y)
                    return e;
            }

            return null;
        }

        private TrackWeaverItem GetWeaverItem(int x, int y)
        {
            var track = Track;
            var trackInfo = track.Get(x, y);

            if (trackInfo == null)
                return null;

            var weaver = _dispatcher.Weaver;
            if (weaver != null)
            {
                var ws = weaver.WovenSeam;
                if (ws != null)
                {
                    foreach (var seam in ws)
                    {
                        if (seam == null)
                            continue;

                        if (seam.TrackObjects.ContainsKey(trackInfo))
                        {
                            return seam;
                        }
                    }
                }
            }

            return null;
        }

        private void JsCallbackOnCellSelected(object sender, int x, int y)
        {
            SelectionX = x;
            SelectionY = y;
            RaisePropertyChanged("SelectionX");
            RaisePropertyChanged("SelectionY");

            SelectionXYvisible = !(x == -1 || y == -1);
            RaisePropertyChanged("SelectionXYvisible");

            if (x == -1 || y == -1)
            {
                Ctx.Send(state =>
                {
                    ShowObjectEdit = false;
                    ItemsS88.Clear();
                    ItemsSwitch.Clear();
                }, null);
                Trace.WriteLine("Selection reset");
                return;
            }

            ShowObjectEdit = true;

            if (Ctx == null)
                return;

            Ctx.Send(state =>
            {
                var dataProvider = _dispatcher.GetDataProvider();
                if (dataProvider == null)
                    return;

                var objItem = GetObject(x, y);
                if (objItem != null)
                {
                    switch (objItem.TypeId())
                    {
                        case 4: // S88
                        {
                            ItemsS88Selection = objItem as S88;
                            SelectionTabIndex = TabIndexS88;

                            var weaveItem = GetWeaveItem(SelectionX, SelectionY);
                            if (weaveItem != null)
                                ItemsS88SelectionPin = weaveItem.Pin;
                        }
                            break;

                        case 5: // switch
                        {
                            ItemsSwitchSelection = objItem as TrackInformation.Switch;
                            SelectionTabIndex = TabIndexSwitch;
                            ItemsS88SelectionPin = -1;
                        }
                            break;
                    }
                }

                foreach (var e in dataProvider.Objects)
                {
                    var ee0 = e as S88;
                    if (ee0 != null)
                        _itemsS88.Add(ee0);

                    var ee1 = e as TrackInformation.Switch;
                    if (ee1 != null)
                        _itemsSwitch.Add(ee1);
                }

            }, null);
        }

        private void JsCallbackOnCellClicked(object o, int x, int y)
        {
            var weaverItem = GetWeaverItem(x, y);

            var objItem = weaverItem?.ObjectItem;

            if (objItem != null)
            {
                switch (objItem.TypeId())
                {
                    case 5:
                    {
                        var switchItem = objItem as TrackInformation.Switch;
                        if (switchItem != null)
                        {
                            if (switchItem.State == 0)
                                switchItem.ChangeDirection(1);
                            else
                                switchItem.ChangeDirection(0);
                        }
                    }
                        break;
                }
            }

            //var track = Track;
            //var trackInfo = track.Get(x, y);

            //if (trackInfo == null)
            //    return;

            //var weaver = _dispatcher.Weaver;
            //if (weaver != null)
            //{
            //    var ws = weaver.WovenSeam;
            //    if (ws != null)
            //    {
            //        foreach (var seam in ws)
            //        {
            //            if (seam == null)
            //                continue;

            //            if (seam.TrackObjects.ContainsKey(trackInfo))
            //            {
            //                var objItem = seam.ObjectItem;

            //                if (objItem != null)
            //                {
            //                    switch (objItem.TypeId())
            //                    {
            //                        case 5:
            //                        {
            //                            var switchItem = objItem as TrackInformation.Switch;
            //                            if (switchItem != null)
            //                            {
            //                                if (switchItem.State == 0)
            //                                    switchItem.ChangeDirection(1);
            //                                else
            //                                    switchItem.ChangeDirection(0);
            //                            }
            //                        }
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

    }
}
