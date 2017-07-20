using System.Windows.Controls;

namespace RailwayEssentialMdi.Views
{
    public partial class LogView : UserControl
    { 
        public LogView()
        {
            InitializeComponent();
        }

        private void Log_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            //Log.Focus();
            Log.CaretIndex = Log.Text.Length;
            Log.ScrollToEnd();
        }
    }
}
