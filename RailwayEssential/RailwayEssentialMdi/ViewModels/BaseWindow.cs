using System;
using RailwayEssentialMdi.Commands;

namespace RailwayEssentialMdi.ViewModels
{
    public class BaseWindow : Bases.ViewModelBase, Interfaces.IContent
    {
        public event EventHandler Closing;

        public virtual string Title { get; set; }

        public virtual RelayCommand CloseCommand { get; }

        public BaseWindow()
        {
            CloseCommand = new RelayCommand(CloseWindow);
        }

        protected virtual void CloseWindow(object p)
        {
            var hander = Closing;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
