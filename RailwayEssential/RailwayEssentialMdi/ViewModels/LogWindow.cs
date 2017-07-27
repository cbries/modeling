using System;
using System.IO;
using System.Text;
using MDIContainer.DemoClient.Commands;
using RailwayEssentialMdi.Entities;

namespace RailwayEssentialMdi.ViewModels
{
    public class LogWindow : BaseWindow
    {
        public enum Mode
        {
            General,
            Commands
        }

        public Mode LogMode { get; set; }

        public override string Title
        {
            get
            {
                if (LogMode == Mode.General)
                    return "General Log";
                return "Command Log";
            }
        }

        public RelayCommand SaveCommand { get; }

        public LogEntity Log { get; }

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

        public LogWindow(LogEntity logMsgs)
        {
            LogMode = Mode.General;
            Log = logMsgs;
            
            SaveCommand = new RelayCommand(SaveCmd);
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
