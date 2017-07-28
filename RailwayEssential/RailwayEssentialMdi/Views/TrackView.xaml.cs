using System;
using System.Windows;
using System.Windows.Controls;
using RailwayEssentialMdi.ViewModels;

namespace RailwayEssentialMdi.Views
{
    public partial class TrackView : UserControl
    {
        public TrackView()
        {
            InitializeComponent();
        }

        private void TrackView_OnInitialized(object sender, EventArgs e)
        {
        }

        private void TrackView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ctx = DataContext as TrackWindow;
            if (ctx == null)
                return;
        }
    }
}
