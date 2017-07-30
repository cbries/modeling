using System.Security.Cryptography;

namespace RailwayEssentialMdi
{
    using System;
    using RailwayEssentialCore;

    public class Configuration : Bases.ViewModelBase, IConfiguration
    {
        private string _ipAddress;
        private UInt16 _port;
        private int _designerColumns;
        private int _designerRows;

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                RaisePropertyChanged("IpAddress");
            }
        }

        public UInt16 Port
        {
            get => _port;
            set
            {
                _port = value;
                RaisePropertyChanged("Port");
            }
        }

        public int DesignerColumns
        {
            get => _designerColumns;
            set
            {
                _designerColumns = value;
                RaisePropertyChanged("DesignerColumns");
            }
        }

        public int DesignerRows
        {
            get => _designerRows;
            set
            {
                _designerRows = value;
                RaisePropertyChanged("DesignerRows");
            }
        }

        public string ThemeName { get; set; }

        public Configuration()
        {
#if DEBUG
            IpAddress = "192.168.178.61";
#else
            IpAddress = "127.0.0.1";
#endif
            Port = 15471;
            DesignerColumns = 50;
            DesignerRows = 50;
            ThemeName = "SpDrS60used";
        }
    }
}
