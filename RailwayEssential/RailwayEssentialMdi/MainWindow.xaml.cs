using System;
using System.ComponentModel;
using System.Diagnostics;
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
        private bool _initialized = false;

        public MainWindow()
        {
            InitializeComponent();

            Unloaded += MainWindow_Unloaded;

            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(keyDown), true);
        }

        private bool _ctrlIsHold = false;

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (!_initialized)
                return;

            if ((e.Key == Key.LeftCtrl && (e.KeyStates & KeyStates.Down) != 0)
                || e.Key == Key.RightCtrl && (e.KeyStates & KeyStates.Down) != 0)
            {
                _ctrlIsHold = true;
            }
            else
            {
                if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                    _ctrlIsHold = false;
            }

            if (e.Key == Key.S && _ctrlIsHold)
                _dataContext?.Project?.Save();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            _initialized = true;
        }

        private void MainWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var m = DataContext as RailwayEssentialModel;

            if (m != null)
            {
                _dataContext = m;

                m.MainView = this;
            }
        }

        private void DockManager_OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (!_initialized)
                return;

            Trace.WriteLine("Document: " + e.Document);
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

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_dataContext == null)
                return;

            _dataContext.Close(null);
        }

        private void TreeView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PropagateTreeViewSelection();
        }

        private void TreeView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                PropagateTreeViewSelection();
            else if (e.Key == Key.T)
                PropagateTestRoute();
            else if (e.Key == Key.Escape)
                _dataContext.ResetBlockRoutePreview();
        }

        private void PropagateTreeViewSelection()
        {
            if (_dataContext == null)
                return;

            var s = Explorer;
            if (s == null)
                return;

            var item = s.SelectedItem;

            if (item is TrackInformation.Locomotive)
                _dataContext.SetCurrentLocomotive(item);
            else if (item is TrackInformation.Switch)
                _dataContext.SetCurrentSwitch(item);
            else if (item is Items.BlockRouteItem)
                _dataContext.ShowBlockRoutePreview(item);
            else
            {
                // ...
            }
        }

        private void PropagateTestRoute()
        {
            if (_dataContext == null)
                return;

            var s = Explorer;
            if (s == null)
                return;

            var item = s.SelectedItem;

            if (item is Items.BlockRouteItem)
                _dataContext.TestBlockRoute(item);
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
