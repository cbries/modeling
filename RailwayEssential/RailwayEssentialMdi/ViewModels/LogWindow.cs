using System;
using System.IO;
using System.Text;
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
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{LogMode}_log",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                try
                {
                    File.WriteAllText(filename, Log.Message, Encoding.UTF8);
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}
