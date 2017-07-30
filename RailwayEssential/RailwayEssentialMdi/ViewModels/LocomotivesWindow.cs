namespace RailwayEssentialMdi.ViewModels
{
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

                UpdateFuncset();

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

            UpdateFuncset();
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
            Speed = 0;
            PromoteSpeed();

            if(Entity != null)
                Entity.ObjectItem.Stop();
        }

        public void PromoteSpeed()
        {
            Entity.ObjectItem.ChangeSpeed(Entity.ObjectItem.Speed);
        }

        public void UpdateFuncset()
        {
            if (LocomotiveView == null)
                return;

            int nrOfFunctions = 0;
            if (_entity != null && _entity.ObjectItem != null)
                nrOfFunctions = _entity.ObjectItem.NrOfFunctions;
            
            for (int i = 0; i < 32; ++i)
            {
                string name = $"F{i}";

                if (string.IsNullOrEmpty(name))
                    continue;

                if (i < nrOfFunctions)
                {
                    if (_entity != null && _entity.ObjectItem != null)
                    {
                        var state = _entity.ObjectItem.Funcset[i];
                        LocomotiveView.SetToggleButton(name, state);
                        LocomotiveView.SetToggleButtonVisibility(name, true);
                    }
                }
                else
                {
                    LocomotiveView.SetToggleButtonVisibility(name, false);
                }
            }
        }
    }
}
