using System;

namespace RailwayEssentialMdi.Entities
{
    public class ObjectEntity : Bases.ViewModelBase
    {
        public event EventHandler Changed;

        private TrackInformation.Item _objectItem;

        public ObjectEntity(TrackInformation.Item objectItem)
            : base()
        {
            _objectItem = objectItem;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }

    }
}
