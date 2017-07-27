using System;

namespace RailwayEssentialMdi.Entities
{
    public class ItemEntity : Bases.ViewModelBase
    {
        public event EventHandler Changed;

        public TrackInformation.Item Entity { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
