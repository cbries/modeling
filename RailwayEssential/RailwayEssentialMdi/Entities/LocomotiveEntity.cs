namespace RailwayEssentialMdi.Entities
{
    using System;

    public class LocomotiveEntity : Bases.ViewModelBase
    {
        public event EventHandler Changed;

        private TrackInformation.Locomotive _objectItem;

        public TrackInformation.Locomotive ObjectItem
        {
            get => _objectItem;
            set
            {
                _objectItem = value;
                RaisePropertyChanged("ObjectItem");
            }
        }

        #region Name

        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        #endregion

        #region ContentId

        private string _contentId = null;
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    RaisePropertyChanged("ContentId");
                }
            }
        }

        #endregion

        #region IsSelected

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        #region IsActive

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

        #endregion

        public LocomotiveEntity() : base()
        {
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
