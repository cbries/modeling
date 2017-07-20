using System;
using MDIContainer.DemoClient.Commands;
using RailwayEssentialMdi.Entities;

namespace RailwayEssentialMdi.ViewModels
{
    public class LogWindow : Bases.ViewModelBase, Interfaces.IContent
    {
        public enum Mode
        {
            General,
            Commands
        }

        public Mode LogMode { get; set; }

        public string Title
        {
            get
            {
                if (LogMode == Mode.General)
                    return "General Log";
                return "Command Log";
            }
        }

        public event EventHandler Closing;

        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }

        public LogMessages Log { get; }

        private bool _autoscroll;

        public bool Autoscroll
        {
            get => _autoscroll;
            set
            {
                _autoscroll = value;
                RaisePropertyChanged("Autoscroll");
            }
        }

        public LogWindow(LogMessages logMsgs)
        {
            LogMode = Mode.General;
            Log = logMsgs;
            CloseCommand = new RelayCommand(CloseWindow);
            SaveCommand = new RelayCommand(SaveCmd);
        }

        private void CloseWindow(object p)
        {
            var hander = Closing;
            hander?.Invoke(this, EventArgs.Empty);
        }

        private void SaveCmd(object p)
        {
            
        }
    }
}
