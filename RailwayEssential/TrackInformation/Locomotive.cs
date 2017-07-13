using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Locomotive : Item
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _protocol;

        public string Protocol
        {
            get => _protocol;
            set
            {
                _protocol = value;
                OnPropertyChanged();
            }
        }

        private int _addr;

        public int Addr
        {
            get => _addr;
            set
            {
                _addr = value;
                OnPropertyChanged();
            }
        }

        public void ToggleFunction(uint nr, bool state)
        {
            int v = state ? 1 : 0;
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request({ObjectId}, control, force)"),
                CommandFactory.Create($"set({ObjectId}, func[{nr}, {v}])"),
                CommandFactory.Create($"release({ObjectId}, control)")
            };

            OnCommandsReady(this, ctrlCmds);
        }

        public void ChangeDirection(uint nr, bool backward)
        {
            int v = backward ? 1 : 0;
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request({ObjectId}, control, force)"),
                CommandFactory.Create($"set({ObjectId}, dir[{v}])"),
                CommandFactory.Create($"release({ObjectId}, control)")
            };

            OnCommandsReady(this, ctrlCmds);
        }

        public void ChangeSpeed(int percentage)
        {
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request({ObjectId}, control, force)"),
                CommandFactory.Create($"set({ObjectId}, speed[{percentage}])"),
                CommandFactory.Create($"release({ObjectId}, control)")
            };

            OnCommandsReady(this, ctrlCmds);
        }

        public override void Parse(List<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg == null)
                    continue;

                if (arg.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                    Name = arg.Parameter[0];
                else if (arg.Name.Equals("protocol", StringComparison.OrdinalIgnoreCase))
                    Protocol = arg.Parameter[0];
                else if (arg.Name.Equals("addr", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Addr = v;
                    else
                        Addr = -1;
                }
            }
        }
    }
}
