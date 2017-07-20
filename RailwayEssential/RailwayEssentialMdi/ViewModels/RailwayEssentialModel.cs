using System.Collections.ObjectModel;
using System.Threading;
using MDIContainer.DemoClient.Commands;
using RailwayEssentialMdi.Bases;
using RailwayEssentialMdi.Entities;
using RailwayEssentialMdi.Interfaces;

namespace RailwayEssentialMdi.ViewModels
{
    public class RailwayEssentialModel : ViewModelBase
    {
        public ObservableCollection<IContent> Windows { get; }

        public RelayCommand ShowLogCommand { get; }
        public RelayCommand ShowCommandLogCommand { get; }

        private LogMessages _logMessagesGeneral = new LogMessages();
        private LogMessages _logMessagesCommands = new LogMessages();

        private static int _counter = 0;

        public RailwayEssentialModel()
        {
            Windows = new ObservableCollection<IContent>();

            ShowLogCommand = new RelayCommand(ShowLog);
            ShowCommandLogCommand = new RelayCommand(ShowCommandLog);

            new Thread(() =>
            {
                for (;;)
                {
                    if (_logMessagesGeneral != null)
                    {
                        _logMessagesGeneral.Add("Message: {0}\r\n", _counter);

                        ++_counter;

                        Thread.Sleep(1000);
                    }
                }
            }) { IsBackground = true }.Start();

            _logMessagesGeneral.Add("Test\r\n");
        }

        public void ShowLog(object p)
        {
            foreach (var item in Windows)
            {
                if (item == null)
                    continue;
                var e = item as LogWindow;
                if (e?.LogMode == LogWindow.Mode.General)
                    return;
            }

            var item2 = new LogWindow(_logMessagesGeneral) { LogMode = LogWindow.Mode.General };
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void ShowCommandLog(object p)
        {
            foreach (var item in Windows)
            {
                if (item == null)
                    continue;
                var e = item as LogWindow;
                if (e?.LogMode == LogWindow.Mode.Commands)
                    return;
            }

            var item2 = new LogWindow(_logMessagesCommands) { LogMode = LogWindow.Mode.Commands };
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }
    }
}
