using RailwayEssentialMdi.Entities;

namespace RailwayEssentialMdi.ViewModels
{
    using System.Diagnostics;
    using Commands;

    public class LocomotivesWindow : BaseWindow
    {
        private LocomotiveEntity _entity;

        public LocomotiveEntity Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                RaisePropertyChanged("Entity");
            }
        }

        public RelayCommand SwitchFncCommand { get; }

        public LocomotivesWindow()
            : base()
        {
            SwitchFncCommand = new RelayCommand(SwitchFnc);
        }

        private void SwitchFnc(object p)
        {
            Trace.WriteLine("Object: " + p);
        }
    }
}
