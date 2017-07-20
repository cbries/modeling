using System.Windows;
using RailwayEssentialMdi.ViewModels;

namespace RailwayEssentialMdi
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new RailwayEssentialModel();
        }
    }
}
