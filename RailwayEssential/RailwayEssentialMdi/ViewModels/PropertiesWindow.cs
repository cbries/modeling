using System;

namespace RailwayEssentialMdi.ViewModels
{
    public class PropertiesWindow : BaseWindow
    {
        public Configuration Entity { get; set; }

        public override string Title => "Properties";

        public PropertiesWindow(Configuration cfg)
            : base()
        {
            Entity = cfg;
        }

        private string _host = "localhost";
        public string Host
        {
            get => _host;
            set
            {
                _host = value;
                RaisePropertyChanged("Host");
            }
        }

        private UInt16 _port = 15471;

        public UInt16 Port
        {
            get => _port;
            set
            {
                _port = value;
                RaisePropertyChanged("Port");
            }
        }
    }
}
