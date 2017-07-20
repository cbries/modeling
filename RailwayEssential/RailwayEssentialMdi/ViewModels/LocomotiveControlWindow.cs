
using System;
using MDIContainer.DemoClient.Commands;

namespace RailwayEssentialMdi.ViewModels
{
    public class LocomotiveControlWindow : Bases.ViewModelBase, Interfaces.IContent
    {
        public string Title => "Locomotive";
        public event EventHandler Closing;

        public RelayCommand CloseCommand { get; }

        public LocomotiveControlWindow()
        {
            CloseCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow(object p)
        {
            var hander = Closing;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
