using System.Collections;
using System.Windows;
using System.Windows.Controls;

using MDIContainer.Control.Events;

namespace MDIContainer.Control
{
    public sealed class MDIContainer : System.Windows.Controls.Primitives.Selector
    {
        private IList InternalItemSource { get; set; }
        internal int MinimizedWindowsCount { get; private set; }

        static MDIContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MDIContainer), new FrameworkPropertyMetadata(typeof(MDIContainer)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var win = new MDIWindow();

            return win;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var window = element as MDIWindow;
            if (window != null)
            {
                window.IsCloseButtonEnabled = InternalItemSource != null;
                window.FocusChanged += OnWindowFocusChanged;
                window.Closing += OnWindowClosing;
                window.WindowStateChanged += OnWindowStateChanged;
                window.Initialize(this);

                Canvas.SetTop(window, 32);
                Canvas.SetLeft(window, 32);

                window.Focus();
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        private void OnWindowStateChanged(object sender, WindowStateChangedEventArgs e)
        {
            if (e.NewValue == WindowState.Minimized)
            {
                MinimizedWindowsCount++;
            }
            else if (e.OldValue == WindowState.Minimized)
            {
                MinimizedWindowsCount--;
            }
        }

        private void OnWindowClosing(object sender, RoutedEventArgs e)
        {
            var window = sender as MDIWindow;
            if (window != null && window.DataContext != null)
            {
                if (InternalItemSource != null)
                {
                    InternalItemSource.Remove(window.DataContext);
                }

                // clear
                window.FocusChanged -= OnWindowFocusChanged;
                window.Closing -= OnWindowClosing;
                window.WindowStateChanged -= OnWindowStateChanged;
                window.DataContext = null;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (newValue != null && newValue is IList)
            {
                InternalItemSource = newValue as IList;
            }
        }

        private void OnWindowFocusChanged(object sender, RoutedEventArgs e)
        {
            if (((MDIWindow)sender).IsFocused)
            {
                SelectedItem = e.OriginalSource;
            }
        }
    }
}
