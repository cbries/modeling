using System.Diagnostics;
using System.IO;
using RailwayEssentialCore;

namespace RailwayEssentialWeb
{
    public partial class TrackViewerControl
    {
        private const string ThemeName = @"\Themes\SpDrS60";
        private const string TrackplansEditor = @"\Trackplans\Webeditor";

        private TrackPlanParser.Track _track;

        public TrackPlanParser.Track Track => _track;

        public string FilePath { get; set; }

        private string _tmpTrackName;

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

                    var col = item.X;
                    var row = item.Y;
                    var symbol = item.IconName;
                    var orientation = item.Orientation;

                    Viewer.ExecuteJs($"simulateClick({col}, {row}, \"{symbol}\", \"{orientation}\");");
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
                    var icon = state ? "sensor-on" : "sensor-off";
                    var orientation = trackItem.Orientation;

                    Viewer.JsCallback.TrackEdit.ChangeSymbol(x, y, icon);
                    Viewer.ExecuteJs($"changeSymbol({x}, {y}, \"{icon}\", \"{orientation}\");");

                    Trace.WriteLine($"CHANGE: {x},{y} -> {icon}");
                }
            }

            return true;
        }
    }
}
