﻿using System.Diagnostics;
using System.IO;
using RailwayEssentialCore;

namespace RailwayEssentialWeb
{
    public partial class TrackViewerControl
    {
        private const string ThemeName = @"\Themes\SpDrS60used";
        private const string TrackplansEditor = @"\Trackplans\Webeditor";

        public ITrackViewerJsCallback JsCallback => Viewer.JsCallback;

        private TrackPlanParser.Track _track;

        public TrackPlanParser.Track Track => _track;

        public string FilePath { get; set; }

        private string _tmpTrackName;

        private Theme.Theme _theme;

        public Theme.Theme Theme
        {
            get
            {
                if (_theme == null)
                {
                    var themePath = ThemeName.ExpandRailwayEssential();
                    var themeDescription = themePath + ".json";
                    _theme = new Theme.Theme();
                    _theme.Load(themeDescription);
                }

                return _theme;
            }
        }

        public TrackViewerControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            TrackPlanParser.TrackPlanParser parser = new TrackPlanParser.TrackPlanParser(FilePath);
            parser.Parse();

            _track = parser.Track;

            Viewer.ViewerReady += delegate (object sender)
            {
                ITrackViewer trackViewer = sender as ITrackViewer;
                if (trackViewer == null)
                    return;

                trackViewer.JsCallback.TrackEdit = _track;

                // load current track
                foreach (var item in _track)
                {
                    if (item == null)
                        continue;

                    var themeItem = Theme.Get(item.ThemeId);
                    if (themeItem != null)
                    {
                        var col = item.X;
                        var row = item.Y;
                        var symbol = Path.GetFileNameWithoutExtension(themeItem.Off.Default);
                        var orientation = item.Orientation;

                        Viewer.ExecuteJs($"simulateClick({col}, {row}, {item.ThemeId}, \"{symbol}\", \"{orientation}\");");
                    }
                }
            };

            _tmpTrackName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "_track.html";
            _tmpTrackName = Path.Combine(TrackplansEditor, _tmpTrackName);
            _tmpTrackName = _tmpTrackName.ExpandRailwayEssential();

            Viewer.WebGenerator = new WebGenerator { ThemeDirectory = ThemeName.ExpandRailwayEssential() };
            GeneratePhysicalTrackViewHtml();
            LoadTrackView();
        }

        private void GeneratePhysicalTrackViewHtml()
        {
            if (Viewer.WebGenerator == null)
                return;

            Viewer.WebGenerator.Generate(_tmpTrackName);
        }

        private void LoadTrackView()
        {
            Viewer.Url = _tmpTrackName;
            Viewer.Reload();
        }

        public bool UpdateUi(TrackWeaver.TrackWeaver weaver)
        {
            Trace.WriteLine(" ** UpdateUi() ** ");
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

                    var state = false;
                    if (checkState != null)
                        state = checkState();

                    var x = trackItem.X;
                    var y = trackItem.Y;
                    var orientation = trackItem.Orientation;

                    int themeId = trackItem.ThemeId;
                    var themeObject = _theme.Get(themeId);
                    string symbol = "";
                    if (state)
                        symbol = themeObject.Active.Default;
                    else
                        symbol = themeObject.Off.Default;

                    Viewer.JsCallback.TrackEdit.ChangeSymbol(x, y, themeId);
                    Viewer.ExecuteJs($"changeSymbol({x}, {y}, {themeId}, \"{orientation}\", \"{symbol}\");");

                    Trace.WriteLine($"CHANGE: {x},{y} -> {symbol}");
                }
            }

            return true;
        }
    }
}
