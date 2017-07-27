
using System;
using System.Diagnostics;
using MDIContainer.DemoClient.Commands;

namespace RailwayEssentialMdi.ViewModels
{
    public class LocomotiveControlWindow : Bases.ViewModelBase, Interfaces.IContent
    {
        public string Title => "Locomotive";

        public event EventHandler Closing;

        public RelayCommand CloseCommand { get; }
        public RelayCommand SwitchFncCommand { get; }

        public LocomotiveControlWindow()
        {
            CloseCommand = new RelayCommand(CloseWindow);
            SwitchFncCommand = new RelayCommand(SwitchFnc);
        }

        private void CloseWindow(object p)
        {
            var hander = Closing;
            hander?.Invoke(this, EventArgs.Empty);
        }

        private void SwitchFnc(object p)
        {
            Trace.WriteLine("Object: " + p);
        }
    }
}
