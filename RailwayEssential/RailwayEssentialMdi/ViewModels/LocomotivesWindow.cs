namespace RailwayEssentialMdi.ViewModels
{
    using System.Threading;
    using System.Windows.Controls.Primitives;
    using Commands;
    using Entities;
    using Interfaces;

    public class LocomotivesWindow : BaseWindow
    {
        public ILocomotiveView LocomotiveView { get; set; }

        private LocomotiveEntity _entity;

        public LocomotiveEntity Entity
        {
            get
            {
                return _entity;
            }

            set
            {
                _entity = value;

                RaisePropertyChanged("Entity");
            }
        }

        public override string Name => Entity != null ? Entity.Name : "-";

        public int Speed
        {
            get => Entity == null ? 0 : Entity.ObjectItem.Speed;
            set
            {
                if (Entity != null)
                {
                    Entity.ObjectItem.Speed = value;
                    RaisePropertyChanged("Speed");
                }
            }
        }

        public RelayCommand SwitchFncCommand { get; }
        public RelayCommand SpeedIncCommand { get; }
        public RelayCommand SpeedDecCommand { get; }
        public RelayCommand StopCommand { get; }

        public LocomotivesWindow()
        {
            SwitchFncCommand = new RelayCommand(SwitchFnc);
            SpeedIncCommand = new RelayCommand(SpeedInc);
            SpeedDecCommand = new RelayCommand(SpeedDec);
            StopCommand = new RelayCommand(Stop);

            _entity?.UpdateUi();
        }

        private void SwitchFnc(object p)
        {
            int index;
            if (!int.TryParse(p.ToString(), out index))
                return;

            var name = $"F{index}";

            if (string.IsNullOrEmpty(name))
                return;

            ToggleButton btn = LocomotiveView.GetToggleButton(name);
            
            if(Entity != null && btn != null && btn.IsChecked.HasValue)
                Entity.ObjectItem.ToggleFunction((uint)index, btn.IsChecked.Value);
        }

        private void SpeedInc(object p)
        {
            if (Entity == null)
                return;
            var v = Entity.ObjectItem.Speed;
            v += 5;
            if (v >= 100)
                v = 100;

            Speed = v;

            PromoteSpeed();
        }

        private void SpeedDec(object p)
        {
            if (Entity == null)
                return;
            var v = Entity.ObjectItem.Speed;
            v -= 5;
            if (v <= 0)
                v = 0;

            Speed = v;

            PromoteSpeed();
        }

        private void Stop(object p)
        {
            Entity.DriveBackward = !Entity.DriveBackward;
            Thread.Sleep(100);
            Entity.DriveBackward = !Entity.DriveBackward;

            Speed = 0;
            PromoteSpeed();
        }

        public void PromoteSpeed()
        {
            Entity.ObjectItem.ChangeSpeed(Entity.ObjectItem.Speed);
        }

        public void UpdateFuncset()
        {
            for (int i = 0; i < 32; ++i)
            {
                string name = $"F{i}";

                if (string.IsNullOrEmpty(name))
                    continue;

                if (_entity != null && _entity.ObjectItem != null)
                {
                    var state = _entity.ObjectItem.Funcset[i];
                    LocomotiveView.SetToggleButton(name, state);
                }
            }
        }
    }
}
