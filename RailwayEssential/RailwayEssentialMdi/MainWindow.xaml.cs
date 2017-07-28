using System.IO;
using System.Windows;
using RailwayEssentialMdi.Interfaces;
using RailwayEssentialMdi.ViewModels;
using Xceed.Wpf.AvalonDock;

namespace RailwayEssentialMdi
{
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow()
        {
            InitializeComponent();

            var m = DataContext as RailwayEssentialModel;
            if (m != null)
                m.MainView = this as IMainView;

            Unloaded += MainWindow_Unloaded;
        }

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var serializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
                serializer.Serialize(@".\AvalonDock.config");
            }
            catch
            {
                // ignore
            }
        }

        #region IMainView

        public void LoadLayout()
        {
            try
            {
                var serializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
                serializer.LayoutSerializationCallback += (s, args) =>
                {
                    args.Content = args.Content;
                };

                if (File.Exists(@".\AvalonDock.config"))
                    serializer.Deserialize(@".\AvalonDock.config");
            }
            catch
            {
                // ignore
            }
        }

        public DockingManager GetDock()
        {
            return dockManager;
        }

        #endregion
    }
}
