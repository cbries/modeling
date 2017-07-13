using System.IO;

namespace RailwayEssentialWeb
{
    public partial class TrackViewerControl
    {
        private const string Testfile = @"C:\temp\trackplan.html";
        private const string TargetDirectory = @"C:\temp\";

        private const string ThemePath =
            @"C:\Users\ChristianRi\Desktop\Github\modeling\RailwayEssential\RailwayEssentialUi\Resources\Theme\SpDrS60";

        //private const string ThemePath =
        //    @"C:\Users\cries\Source\Repos\modeling\RailwayEssential\RailwayEssentialUi\Resources\Theme\SpDrS60";

        private FileSystemWatcher _watcher;

        public TrackViewerControl()
        {
            InitializeComponent();

            Viewer.WebGenerator = new WebGenerator { ThemeDirectory = ThemePath };
            GeneratePhysicalTrackViewHtml();
            StartWatcher();
            LoadTrackView();
        }

        private void GeneratePhysicalTrackViewHtml()
        {
            if (Viewer.WebGenerator == null)
                return;

            Viewer.WebGenerator.Generate(TargetDirectory);
        }

        public bool StartWatcher()
        {
            try
            {
                var dirname = Path.GetDirectoryName(Testfile);
                if (string.IsNullOrEmpty(dirname))
                    return false;

                if (_watcher == null)
                {
                    _watcher = new FileSystemWatcher(dirname);
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
            Viewer.Url = Testfile;
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
