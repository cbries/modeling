using System;
using System.Diagnostics;
using System.IO;
using RailwayEssentialCore;

namespace RailwayEssentialWeb
{
    public partial class TrackViewerControl
    {
        private const string ThemeName = @"\Themes\SpDrS60";
        private const string TrackplansDirectory = @"\Trackplans";
        private const string TrackplansEditor = @"\Trackplans\Webeditor";

        private TrackPlanParser.Track _track = null;

        public TrackPlanParser.Track Track => _track;

        public string Trackname { get; set; }

        public string Trackpath
        {
            get
            {
                if (string.IsNullOrEmpty(Trackname))
                    return null;

                return Path.Combine(TrackplansDirectory.ExpandRailwayEssential(), Trackname);
            }
        }

        private FileSystemWatcher _watcher;

        private readonly string _tmpTrackName;

        public TrackViewerControl()
        {
            InitializeComponent();
            
            Viewer.ViewerReady += delegate(object sender)
            {
                ITrackViewer trackViewer = sender as ITrackViewer;
                if (trackViewer == null)
                    return;

                TrackPlanParser.TrackPlanParser parser = new TrackPlanParser.TrackPlanParser(Trackpath);
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

                    Viewer.ExecuteJs($"simulateClick({col}, {row}, \"{symbol}\");");
                }
            };

            _tmpTrackName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "_track.html";
            _tmpTrackName = Path.Combine(TrackplansEditor, _tmpTrackName);
            _tmpTrackName = _tmpTrackName.ExpandRailwayEssential();

            Viewer.WebGenerator = new WebGenerator { ThemeDirectory = ThemeName.ExpandRailwayEssential() };
            GeneratePhysicalTrackViewHtml();
            StartWatcher();
            LoadTrackView();
        }

        private void GeneratePhysicalTrackViewHtml()
        {
            if (Viewer.WebGenerator == null)
                return;

            Viewer.WebGenerator.Generate(_tmpTrackName);
        }

        public bool StartWatcher()
        {
            try
            {
                var dirname = Path.GetDirectoryName(_tmpTrackName);
                if (string.IsNullOrEmpty(dirname))
                    return false;

                if (_watcher == null)
                {
                    _watcher = new FileSystemWatcher(dirname, "*_track.html");
                    _watcher.Deleted += WatcherOnDeleted;
                    _watcher.Created += WatcherOnCreated;
                    _watcher.Changed += WatcherOnChanged;
                }

                _watcher.EnableRaisingEvents = true;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StopWatcher()
        {
            try
            {
                if (_watcher != null)
                {
                    _watcher.EnableRaisingEvents = false;
                    _watcher.Deleted -= WatcherOnDeleted;
                    _watcher.Created -= WatcherOnCreated;
                    _watcher.Changed -= WatcherOnChanged;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void LoadTrackView()
        {
            Viewer.Url = _tmpTrackName;
            Viewer.Reload();
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            LoadTrackView();
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            LoadTrackView();
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            LoadTrackView();
        }
    }
}
