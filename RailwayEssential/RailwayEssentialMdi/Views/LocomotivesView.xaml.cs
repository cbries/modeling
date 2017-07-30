using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using RailwayEssentialMdi.Interfaces;

namespace RailwayEssentialMdi.Views
{
    public partial class LocomotivesView : UserControl, ILocomotiveView
    {
        private ViewModels.LocomotivesWindow _dataContext;

        public LocomotivesView()
        {
            InitializeComponent();
        }

        private void LocomotivesView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var m = DataContext as ViewModels.LocomotivesWindow;
            if (m != null)
            {
                _dataContext = m;

                m.LocomotiveView = this;

                _dataContext.UpdateFuncset();
            }
        }

        #region ILocomotiveView

        public ToggleButton GetToggleButton(string name)
        {
            foreach (var c in FncButtons.Children)
            {
                if (c == null)
                    continue;
                var tbn = c as ToggleButton;
                if (tbn == null)
                    continue;

                if (tbn.Content.Equals(name))
                    return tbn;
            }

            return null;
        }

        public void SetToggleButton(string name, bool state)
        {
            var btn = GetToggleButton(name);
            if (btn != null)
                btn.IsChecked = state;
        }

        public void SetToggleButtonVisibility(string name, bool visible)
        {
            var btn = GetToggleButton(name);
            if (btn != null)
            {
                if(visible)
                    btn.Visibility = Visibility.Visible;
                else
                    btn.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        private void SpeedSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_dataContext == null)
                return;
            var v = (int) SpeedSlider.Value;
            if (_dataContext.Speed != v)
                _dataContext.Speed = v;
        }

        private void SpeedSlider_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (_dataContext == null)
                return;

            _dataContext.PromoteSpeed();
        }
    }
}
