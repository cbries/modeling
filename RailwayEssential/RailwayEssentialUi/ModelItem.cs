using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RailwayEssentialUi
{
    public class ModelItem : MenuItem
    {
        public object Object { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public string Typename
        {
            get
            {
                var item = Object as TrackInformation.IItem;
                if(item == null)
                    return "-";
                return item.ToString();
            }
        }

        private BitmapSource _iconSource = null;

        public BitmapSource IconSource
        {
            get
            {
                if (_iconSource != null)
                    return _iconSource;

                UpdateIconPath();

                return _iconSource;
            }
        }

        private void UpdateIconPath()
        {
            string BasePackUrlsPath = @"pack://application:,,,/RailwayEssentialUi;component/Resources/";

            var item = Object as TrackInformation.IItem;
            if (item == null)
                _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "unknown.png"));
            else
            {
                if(item is TrackInformation.Locomotive)
                    _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "train.png"));
                else if(item is TrackInformation.Switch)
                    _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "switch.png"));
                else if(item is TrackInformation.Route)
                    _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "route.png"));
                else if(item is TrackInformation.S88)
                    _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "s88.png"));
                else
                    _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "unknown.png"));
            }
        }

        public ModelItem()
        { }
    }
}
