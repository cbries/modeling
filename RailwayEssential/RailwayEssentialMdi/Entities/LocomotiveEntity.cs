namespace RailwayEssentialMdi.Entities
{
    using System;
    using System.Diagnostics;

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

                if (_objectItem != null)
                    _objectItem.Ctx = Model;

                UpdateUi();

                RaisePropertyChanged("ObjectItem");
                RaisePropertyChanged("Name");
            }
        }
        
        #region Name

        public string Name
        {
            get => _objectItem != null ? _objectItem.Name : "-";

            set
            {
                if (_objectItem != null)
                {
                    _objectItem.Name = value;
                    _objectItem.ChangeName(value);
                }

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

        #region IsEnabled

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }

        #endregion
        
        private bool _driveForward;
        private bool _driveBackward;

        public bool DriveForward
        {
            get => _driveForward;
            set
            {
                _driveForward = value;
                _driveBackward = !value;

                ObjectItem.ChangeDirection((uint)ObjectItem.ObjectId, !value);

                if (_objectItem != null)
                    _objectItem.Speed = 0;

                UpdateUi();
            }
        }

        public bool DriveBackward
        {
            get => _driveBackward;
            set
            {
                _driveBackward = value;
                _driveForward = !value;

                ObjectItem.ChangeDirection((uint)ObjectItem.ObjectId, value);

                if (_objectItem != null)
                    _objectItem.Speed = 0;

                UpdateUi();
            }
        }

        public void UpdateUi()
        {
            Trace.WriteLine(" *** UpdateUi() of Locomotive *** ");

            if (_objectItem == null)
            {
                IsEnabled = false;

                return;
            }

            IsEnabled = true;

            if (ObjectItem != null && ObjectItem.Direction == 1)
            {
                _driveBackward = true;
                _driveForward = false;
            }
            else
            {
                _driveBackward = false;
                _driveForward = true;
            }

            if (ObjectItem != null)
            {
                // ...
            }

            RaisePropertyChanged("DriveForward");
            RaisePropertyChanged("DriveBackward");

            if (ObjectItem != null)
            {
                ObjectItem.RaisePropertyChange("Speed");
                ObjectItem.RaisePropertyChange("ObjectItem.Speed");
                ObjectItem.UpdateTitle();
                ObjectItem.UpdateSubTitle();
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
