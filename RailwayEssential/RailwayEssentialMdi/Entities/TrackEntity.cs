using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;
using RailwayEssentialMdi.DataObjects;
using RailwayEssentialWeb;
using TrackWeaver;

namespace RailwayEssentialMdi.Entities
{
    public partial class TrackEntity : Bases.ViewModelBase, IPersist
    {
        public event EventHandler Changed;

        public const string ToolContentId = "TrackEntityTool";

        private bool _initialized;
        private string _tmpTrackName;
        private Theme.Theme _theme;
        private Dispatcher.Dispatcher _dispatcher;
        private ITrackViewer _trackViewer;
        private TrackPlanParser.Track _track;
        private WebGenerator _webGenerator;

        public string TrackObjectFilepath { get; set; }

        internal SynchronizationContext Ctx { get; set; }

        public ProjectTrack ProjectTrack { get; set; }

        internal ITrackViewer Viewer => _trackViewer;

        internal Dispatcher.Dispatcher Dispatcher => _dispatcher;

        #region Name

        public string Name
        {
            get
            {
                if (ProjectTrack == null)
                    return "-";

                return ProjectTrack.Name;
            }

            set
            {
                if (ProjectTrack != null)
                    ProjectTrack.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        #endregion

        #region ContentId

        private string _contentId = null;
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    RaisePropertyChanged("ContentId");
                }
            }
        }

        #endregion

        #region IsSelected

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        #region IsActive

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

        #endregion

        #region CanClose

        private bool _canClose = false;
        public bool CanClose
        {
            get
            {
                return _canClose;
            }
            set
            {
                if (_canClose != value)
                {
                    _canClose = value;
                    RaisePropertyChanged("CanClose");
                    RaisePropertyChanged("CanEdit");
                }
            }
        }

        #endregion

        #region CanEdit

        public bool CanEdit
        {
            get
            {
                return !_canClose;
            }
        }

        #endregion

        public Theme.Theme Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                RaisePropertyChanged("Theme");
            }
        }

        public string TrackEditor { get; private set; }

        public TrackPlanParser.Track Track => _track;

        public IWebGenerator WebGenerator { get; set; }

        public Configuration Cfg { get; set; }

        public TrackEntity(Dispatcher.Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            ContentId = ToolContentId;
            TrackEditor = Utils.TrackplansEditor.ExpandRailwayEssential();
        }

        public TrackEntity Clone()
        {
            var e = new TrackEntity(_dispatcher)
            {
                TrackObjectFilepath = TrackObjectFilepath,
                Theme = _theme,
                Ctx = Ctx,
                ProjectTrack = ProjectTrack,
                Cfg = Cfg,
                Model = Model
            };

            e.Initialize();

            return e;
        }

        public bool Initialize()
        {
            if (_initialized)
                return true;

            _initialized = true;

            TrackPlanParser.TrackPlanParser parser = new TrackPlanParser.TrackPlanParser(TrackObjectFilepath);

            parser.Parse();

            _track = parser.Track;

            _tmpTrackName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "_track.html";
            _tmpTrackName = Path.Combine(Utils.TrackplansEditor, _tmpTrackName);
            _tmpTrackName = _tmpTrackName.ExpandRailwayEssential();

            _webGenerator = new WebGenerator(_theme)
            {
                Columns = Cfg.DesignerColumns,
                Rows = Cfg.DesignerRows
            };

            GeneratePhysicalTrackViewerUi();

            return true;
        }

        public bool ViewerReady()
        {
            if (_trackViewer != null && _trackViewer.JsCallback != null)
            {
                _trackViewer.JsCallback.CellEdited += JsCallbackOnCellEdited;
                _trackViewer.JsCallback.CellClicked += JsCallbackOnCellClicked;
                _trackViewer.JsCallback.CellSelected += JsCallbackOnCellSelected;
                _trackViewer.JsCallback.EditModeChanged += JsCallbackOnEditModeChanged;
                _trackViewer.JsCallback.TrackEdit = _track;
            }

            JArray arClicks = new JArray();

            // load current track
            foreach (var item in _track)
            {
                if (item == null)
                    continue;

                var themeItem = _theme.Get(item.ThemeId);
                if (themeItem != null)
                {
                    var symbol = Path.GetFileNameWithoutExtension(themeItem.Off.Default);
                    var orientation = item.Orientation;

                    JObject o = new JObject
                    {
                        ["col"] = item.X,
                        ["row"] = item.Y,
                        ["themeId"] = item.ThemeId,
                        ["symbol"] = symbol,
                        ["orientation"] = orientation
                    };

                    arClicks.Add(o);
                }
            }

            if (_trackViewer != null)
                _trackViewer.ExecuteJs($"simulateClick2({arClicks.ToString(Formatting.None)});");

            UpdateAllVisualBlocks();
            UpdateAllVisualIds(Model.IsVisualLabelActivated);

            return true;
        }

        public void PromoteViewer(ITrackViewer trackViewer)
        {
            _trackViewer = trackViewer;
            _trackViewer.SetUrl(_tmpTrackName);
        }

        private void JsCallbackOnEditModeChanged(object o, bool editState)
        {
            if (!editState)
            {
                ShowObjectEdit = false;
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }

        private void GeneratePhysicalTrackViewerUi()
        {
            if (_webGenerator == null)
                return;

            _webGenerator.Generate(_tmpTrackName);
        }
        
        public bool UpdateTrackViewerUi(TrackWeaver.TrackWeaver weaver)
        {
            var sw = StopWatch.Create();

            try
            {
                if (weaver == null)
                    return false;

                var ws = weaver.WovenSeam;
                if (ws == null)
                    return false;

                foreach (var seam in ws)
                {
                    if (seam == null)
                        continue;

                    if (seam.TrackObjects.Count == 0)
                        continue;

                    foreach (var trackItem in seam.TrackObjects.Keys)
                    {
                        if (trackItem == null)
                            continue;

                        var checkState = seam.TrackObjects[trackItem];

                        TrackCheckResult checkResult = null;
                        if (checkState != null)
                            checkResult = checkState();

                        var x = trackItem.X;
                        var y = trackItem.Y;
                        var orientation = trackItem.Orientation;

                        int themeId = trackItem.ThemeId;
                        var themeObject = _theme.Get(themeId);
                        string symbol = "";

                        switch (seam.ObjectItem.TypeId())
                        {
                            case 1: // Locomotive
                                continue;

                            case 2: // Ecos2
                                continue;

                            case 3: // Route
                                continue;

                            case 4: // S88
                                {
                                    bool rS88 = checkResult?.State != null && checkResult.State.Value;

                                    if (rS88)
                                    {
                                        if (seam.ObjectItem.IsRouted)
                                            symbol = themeObject.Active.Route;
                                        else
                                            symbol = themeObject.Active.Default;
                                    }
                                    else
                                    {
                                        if (seam.ObjectItem.IsRouted)
                                            symbol = themeObject.Off.Route;
                                        else
                                            symbol = themeObject.Off.Default;
                                    }
                                }
                                break;

                            case 5: // Switch
                                {
                                    if (checkResult != null && checkResult.Direction.HasValue)
                                    {
                                        var direction = checkResult.Direction.Value;

                                        var objS = seam.ObjectItem as TrackInformation.Switch;
                                        if (objS != null && objS.InvertCommand)
                                        {
                                            if (direction == TrackCheckResult.SwitchDirection.Straight)
                                                direction = TrackCheckResult.SwitchDirection.Turn;
                                            else
                                                direction = TrackCheckResult.SwitchDirection.Straight;
                                        }

                                        if (direction == TrackCheckResult.SwitchDirection.Straight)
                                        {
                                            if (seam.ObjectItem.IsRouted)
                                                symbol = themeObject.Active.Route;
                                            else
                                                symbol = themeObject.Active.Default;
                                        }
                                        else if (direction == TrackCheckResult.SwitchDirection.Turn)
                                        {
                                            if (seam.ObjectItem.IsRouted)
                                                symbol = themeObject.Off.Route;
                                            else
                                                symbol = themeObject.Off.Default;
                                        }
                                        else
                                        {
                                            Trace.WriteLine("<Switch> Unknown direction: " + direction);
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }

                        if (_trackViewer != null && _trackViewer.JsCallback != null)
                        {
                            _trackViewer.JsCallback.TrackEdit.ChangeSymbol(x, y, themeId);
                            _trackViewer.ExecuteJs($"changeSymbol({x}, {y}, {themeId}, \"{orientation}\", \"{symbol}\");");
                        }

                        //Trace.WriteLine($"CHANGE: {x},{y} -> {themeId} | {symbol} | {orientation}");
                    }
                }
            }
            catch (Exception ex)
            {
                // ignore
            }
            finally
            {
                sw.Stop();
                //sw.Show("UpdateUi()");
            }

            return true;
        }

        #region IPersist

        public bool Save()
        {
            return Save(TrackObjectFilepath);
        }

        public bool Save(string targetFilepath)
        {
            if (_track == null)
                return false;

            try
            {
                var trackObject = Track.GetJson();
                if (trackObject != null)
                    File.WriteAllText(targetFilepath, trackObject.ToString(Formatting.Indented));

                return true;
            }
            catch (Exception ex)
            {
                var logger = _dispatcher.Logger;
                if (logger != null)
                    logger.Log("<TrackEntity> " + ex.Message + "\r\n");

                return false;
            }
        }

        #endregion
    }
}
