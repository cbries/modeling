using System.IO;
using System.Windows;
using System.Windows.Input;
using RailwayEssentialMdi.Interfaces;
using RailwayEssentialMdi.ViewModels;
using Xceed.Wpf.AvalonDock;

namespace RailwayEssentialMdi
{
    public partial class MainWindow : Window, IMainView
    {
        private RailwayEssentialModel _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            var m = DataContext as RailwayEssentialModel;
            if (m != null)
            {
                _dataContext = m;
                m.MainView = this;
            }

            Unloaded += MainWindow_Unloaded;
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
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

        private void TreeView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PropagateTreeViewSelection();
        }

        private void TreeView_OnKeyDown(object sender, KeyEventArgs e)
        {
            PropagateTreeViewSelection();
        }

        private void PropagateTreeViewSelection()
        {
            if (_dataContext == null)
                return;

            var s = treeView;
            if (s == null)
                return;

            var item = s.SelectedItem;

            if (item is TrackInformation.Locomotive)
                _dataContext.SetCurrentLocomotive(item);
            else if (item is TrackInformation.Switch)
                _dataContext.SetCurrentSwitch(item);
            else
            {
                // ...
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
