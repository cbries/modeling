﻿
using System;
using System.Collections.Generic;
using RailwayEssentialCore;
using RailwayEssentialMdi.ViewModels;
using TrackPlanParser;

namespace RailwayEssentialMdi.Entities
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using TrackWeaver;
    using TrackInformation;

    public partial class TrackEntity
    {
        private const int TabIndexGeneral = 0;
        private const int TabIndexS88 = 1;
        private const int TabIndexSwitch = 2;
        private const int TabIndexConnector = 3;
        private const int TabIndexBlock = 4;

        private TrackInfo _trackInfoSelection;
        private S88 _itemS88Selection;
        private TrackInformation.Switch _itemSwitchSelection;
        private int _itemsS88SelectionPin;
        private bool _connectorVisible = true;
        private int _connectorIdentifier = -1;
        private int _blockGroupIdentifier;
        private TrackInformation.Locomotive _blockCurrentLocomotive;
        private List<string> _availableBlocks = new List<string>();
        private List<string> _availableSensors = new List<string>();
        private List<TrackInformation.Item> _availableLocomotives = new List<Item>();

        public TrackInfo TrackInfoSelection
        {
            get => _trackInfoSelection;
            set
            {
                _trackInfoSelection = value;
                RaisePropertyChanged("TrackInfoSelection");
                RaisePropertyChanged("TrackInfoSelectionName");
                RaisePropertyChanged("TrackInfoSelectionDescription");
            }
        }

        public string TrackInfoSelectionName
        {
            get => _trackInfoSelection != null ? _trackInfoSelection.Name : "";
            set
            {
                if (_trackInfoSelection != null)
                {
                    _trackInfoSelection.Name = value;

                    UpdateVisualId(_trackInfoSelection, Model.IsVisualLabelActivated);
                }

                RaisePropertyChanged("TrackInfoSelection");
                RaisePropertyChanged("TrackInfoSelectionName");
            }
        }

        public string TrackInfoSelectionDescription
        {
            get => _trackInfoSelection != null ? _trackInfoSelection.Description : "";
            set
            {
                if (_trackInfoSelection != null)
                    _trackInfoSelection.Description = value;
                RaisePropertyChanged("TrackInfoSelection");
                RaisePropertyChanged("TrackInfoSelectionDescription");
            }
        }
        
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

        public int BlockGroupIdentifier
        {
            get => _blockGroupIdentifier;
            set
            {
                _blockGroupIdentifier = value;

                if (_trackInfoSelection != null)
                {
                    _trackInfoSelection.SetOption("blockGroupIdentifier", $"{value}");
                }

                RaisePropertyChanged("BlockGroupIdentifier");
            }
        }

        public TrackInformation.Locomotive BlockCurrentLocomotive
        {
            get => _blockCurrentLocomotive;
            set
            {
                _blockCurrentLocomotive = value;

                if (_trackInfoSelection != null && _blockCurrentLocomotive != null)
                    _trackInfoSelection.SetOption("blockCurrentLocomotive", $"{_blockCurrentLocomotive.ObjectId}");

                UpdateAllVisualBlocks();

                RaisePropertyChanged("BlockCurrentLocomotive");
            }
        }

        public IList<string> AvailableBlocks
        {
            get
            {
                lock (_availableBlocks)
                {
                    _availableBlocks.Clear();

                    if (Track == null)
                        return _availableBlocks;

                    foreach (var e in Track)
                    {
                        if (e == null)
                            continue;

                        if (!Globals.BlockIds.Contains(e.ThemeId))
                            continue;

                        if (string.IsNullOrEmpty(e.Name))
                            _availableBlocks.Add(e.ToString());
                        else
                            _availableBlocks.Add(e.Name);
                    }

                    _availableBlocks.Insert(0, "--");

                    return _availableBlocks;
                }
            }
            set
            {
                lock (_availableBlocks)
                {
                    _availableBlocks = (List<string>) value;
                    RaisePropertyChanged("AvailableBlocks");
                }
            }
        }

        public IList<string> AvailableSensors
        {
            get
            {
                lock (_availableSensors)
                {
                    _availableSensors.Clear();

                    if (Track == null)
                        return _availableSensors;

                    foreach (var e in Track)
                    {
                        if (e == null)
                            continue;

                        if (!Globals.SensorIds.Contains(e.ThemeId))
                            continue;

                        if (string.IsNullOrEmpty(e.Name))
                            _availableSensors.Add(e.ToString());
                        else
                            _availableSensors.Add(e.Name);
                    }

                    _availableSensors.Insert(0, "--");

                    return _availableSensors;
                }
            }
            set
            {
                lock (_availableSensors)
                {
                    _availableSensors = (List<string>)value;
                    RaisePropertyChanged("AvailableSensors");
                }
            }
        }

        public IList<TrackInformation.Item> AvailableLocomotives
        {
            get
            {
                _availableLocomotives.Clear();

                if (Model == null)
                    return _availableLocomotives;

                var m = Model as RailwayEssentialModel;
                if (m == null)
                    return _availableLocomotives;

                foreach (var e in _dispatcher.GetDataProvider().Objects)
                {
                    if (e == null)
                        continue;

                    if (e.TypeId() == 1)
                        _availableLocomotives.Add(e as TrackInformation.Locomotive);
                }

                return _availableLocomotives;
            }
            set
            {
                _availableLocomotives = (List<Item>) value;
                RaisePropertyChanged("AvailableLocomotives");
            }
        }

        public int ConnectorIdentifier
        {
            get => _connectorIdentifier;
            set
            {
                _connectorIdentifier = value;

                if (_trackInfoSelection != null)
                {
                    _trackInfoSelection.SetOption("connectorIdentifier", $"{value}");
                }

                RaisePropertyChanged("ConnectorIdentifier");
            }
        }

        public bool ConnectorVisible
        {
            get => _connectorVisible;
            set
            {
                _connectorVisible = value;
                RaisePropertyChanged("ConnectorVisible");
            }
        }

        public int SelectionX { get; private set; }
        public int SelectionY { get; private set; }
        public bool SelectionXYvisible { get; private set; }

        private int _selectionTabIndex = TabIndexGeneral;

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

            if (TrackInfoSelection != null)
            {
                if (string.IsNullOrEmpty(TrackInfoSelection.Name))
                    trackInfo.Name = "";
                else
                    trackInfo.Name = TrackInfoSelection.Name.Trim();

                if (string.IsNullOrEmpty(TrackInfoSelection.Description))
                    trackInfo.Description = "";
                else
                    trackInfo.Description = TrackInfoSelection.Description.Trim();
            }

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

            prj?.Save();
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

                var trackInfo = Track.Get(x, y);
                if (trackInfo != null)
                    TrackInfoSelection = trackInfo;

                var objItem = GetObject(x, y);

                if (objItem != null)
                {
                    //ConnectorVisible = false;
                    //SelectionTabIndex = 0;
                    
                    switch (objItem.TypeId())
                    {
                        case 4: // S88
                        {
                            ItemsS88Selection = objItem as S88;
                            //SelectionTabIndex = TabIndexS88;

                            var weaveItem = GetWeaveItem(SelectionX, SelectionY);
                            if (weaveItem != null)
                                ItemsS88SelectionPin = weaveItem.Pin;
                        }
                            break;

                        case 5: // switch
                        {
                            ItemsSwitchSelection = objItem as TrackInformation.Switch;
                            //SelectionTabIndex = TabIndexSwitch;
                            ItemsS88SelectionPin = -1;
                        }
                            break;

                        default:
                        {
                            ItemsS88Selection = null;
                            ItemsSwitchSelection = null;
                            //SelectionTabIndex = TabIndexGeneral;
                            ItemsS88SelectionPin = -1;
                        }
                            break;
                    }
                }
                else
                {
                    // Is Connector?
                    if (Track != null)
                    {
                        if (TrackInfoSelection != null)
                        {
                            var themeId = TrackInfoSelection.ThemeId;
                            if (themeId > 0)
                            {

                                var type = Globals.GetThemeType(themeId);

                                switch (type)
                                {
                                    case Globals.ThemeIdType.Connector:
                                        {
                                            // show Connector's configuration tab    

                                            //SelectionTabIndex = TabIndexConnector;
                                            //ConnectorVisible = true;

                                            var opt = TrackInfoSelection.GetOption("connectorIdentifier");

                                            if (!string.IsNullOrEmpty(opt))
                                            {
                                                int v;
                                                if (int.TryParse(opt, out v))
                                                    ConnectorIdentifier = v;
                                                else
                                                    ConnectorIdentifier = 1;
                                            }
                                            else
                                            {
                                                ConnectorIdentifier = -1;
                                            }
                                        }
                                        break;

                                    case Globals.ThemeIdType.Block:
                                        {
                                            //SelectionTabIndex = TabIndexBlock;

                                            #region blockGroupIdentifier

                                            var opt = TrackInfoSelection.GetOption("blockGroupIdentifier");

                                            if (!string.IsNullOrEmpty(opt))
                                            {
                                                int v;
                                                if (int.TryParse(opt, out v))
                                                    BlockGroupIdentifier = v;
                                                else
                                                    BlockGroupIdentifier = 1;
                                            }
                                            else
                                            {
                                                BlockGroupIdentifier = -1;
                                            }

                                            #endregion

                                            #region blockCurrentLocomotive

                                            opt = TrackInfoSelection.GetOption("blockCurrentLocomotive");

                                            if (!string.IsNullOrEmpty(opt))
                                            {
                                                int objectId = -1;
                                                if (int.TryParse(opt, out objectId))
                                                {
                                                    BlockCurrentLocomotive = dataProvider.GetObjectBy(objectId) as Locomotive;
                                                }
                                            }
                                            else
                                            {
                                                BlockCurrentLocomotive = null;
                                            }

                                            #endregion
                                        }
                                        break;

                                    default:
                                        //ConnectorVisible = false;
                                        //SelectionTabIndex = 0;
                                        break;
                                }
                            }
                        }
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

        private void JsCallbackOnCellEdited(object o, EventArgs ev)
        {
            if(Dispatcher != null && Dispatcher.Model != null)
                Dispatcher.Model.SetDirty(true);

            JsonObjectEventArgs evObj = ev as JsonObjectEventArgs;
            if (evObj != null && evObj.GetData() != null)
            {
                int x = -1;
                int y = -1;
                int themeId = -1;

                var data = evObj.GetData();

                if (data["x"] != null)
                    x = (int) data["x"];
                if (data["y"] != null)
                    y = (int) data["y"];
                //if (data["themeId"] != null)
                //    themeId = (int) data["themeId"];

                var state = false;
                if(_dispatcher != null && _dispatcher.Model != null)
                    state = _dispatcher.Model.IsVisualLabelActivated;

                UpdateVisualId(x, y, state);
            }
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
        }

        public void UpdateVisualId(int x, int y, bool show=true)
        {
            if (x != -1 && y != -1)
            {
                var item = _track.Get(x, y);

                if (item != null)
                {
                    if (Viewer != null)
                    {
                        if(show)
                            Viewer.ExecuteJs($"changeItemIdMarker({x}, {y}, \"{item.Name}\");");
                        else
                            Viewer.ExecuteJs($"changeItemIdMarker({x}, {y}, \" \");");
                    }
                }
            }
        }

        public void UpdateVisualId(TrackInfo info, bool show=true)
        {
            var x = info.X;
            var y = info.Y;
            var themeId = info.ThemeId;

            if (x != -1 && y != -1 && themeId != -1)
            {
                var item = _track.Get(x, y);

                if (item != null)
                {
                    if (Viewer != null)
                    {
                        if(show)
                            Viewer.ExecuteJs($"changeItemIdMarker({x}, {y}, '{item.Name}');");
                        else
                            Viewer.ExecuteJs($"changeItemIdMarker({x}, {y}, ' ');");
                    }
                }
            }
        }

        public void UpdateAllVisualIds(bool state)
        {
            if (_track == null)
                return;

            foreach (var item in _track)
            {
                if (item == null)
                    continue;

                UpdateVisualId(item, state);
            }
        }

        public void UpdateVisualBlock(TrackInfo info)
        {
            var x = info.X;
            var y = info.Y;
            var themeId = info.ThemeId;

            if (x != -1 && y != -1 && themeId != -1)
            {
                var item = _track.Get(x, y);

                if (item != null)
                {
                    var opt = item.GetOption("blockCurrentLocomotive");
                    if (string.IsNullOrEmpty(opt))
                    {
                        Viewer.ExecuteJs($"changeLocnameMarker({x}, {y}, ' ');");
                    }
                    else
                    {
                        int objectId = -1;
                        if (int.TryParse(opt, out objectId))
                        {
                            var dataProvider = _dispatcher.GetDataProvider();
                            if (dataProvider == null)
                                return;

                            var loc = dataProvider.GetObjectBy(objectId) as Locomotive;
                            if (loc != null)
                            {
                                if (Viewer != null)
                                    Viewer.ExecuteJs($"changeLocnameMarker({x}, {y}, '{loc.Name}');");
                            }
                        }
                        else
                        {
                            Viewer.ExecuteJs($"changeLocnameMarker({x}, {y}, 'FAILURE');");
                        }
                    }
                }
                else
                {
                    Viewer.ExecuteJs($"changeLocnameMarker({x}, {y}, ' ');");
                }
            }
        }

        public void UpdateAllVisualBlocks()
        {
            if (_track == null)
                return;

            foreach (var item in _track)
            {
                if (item == null)
                    continue;

                if(Globals.BlockIds.Contains(item.ThemeId))
                    UpdateVisualBlock(item);
            }
        }
    }
}
