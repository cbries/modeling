using System.Windows;

namespace RailwayEssentialUi
{
    public partial class MainWindow : Window
    {
        private readonly RailwayEssentialCore.Configuration _cfg;
        private readonly Dispatcher.Dispatcher _dispatcher;

        public MainWindow()
        {
            InitializeComponent();

            _cfg = new RailwayEssentialCore.Configuration();
            _dispatcher = new Dispatcher.Dispatcher {Configuration = _cfg};
        }

        private void TreeViewModel_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
