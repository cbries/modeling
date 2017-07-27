using System.Threading;
using System.Windows;
using RailwayEssentialMdi.ViewModels;

namespace RailwayEssentialMdi
{
    public partial class MainWindow : Window
    {
        private SynchronizationContext _ctx = null;

        public MainWindow()
        {
            InitializeComponent();

            _ctx = SynchronizationContext.Current;

            DataContext = new RailwayEssentialModel();
        }
    }
}
