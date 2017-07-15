using System;
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

        private readonly string _tmpTrackName;

        public TrackViewerControl()
        {
            InitializeComponent();
            
            Viewer.ViewerReady += delegate(object sender)
            {
                ITrackViewer trackViewer = sender as ITrackViewer;
                if (trackViewer == null)
                    return;

                TrackPlanParser.TrackPlanParser parser = new TrackPlanParser.TrackPlanParser(FilePath);
                parser.Parse();

                _track = parser.Track;

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
    }
}
