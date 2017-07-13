using System;
using System.Collections.Generic;
using System.Linq;
using Ecos2Core;

namespace TrackInformation
{
    public class S88 : Item
    {
        private int _index;

        /// <summary> the position within the S88 bus </summary>
        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }

        private int _ports;

        public int Ports
        {
            get => _ports;
            set
            {
                _ports = value;
                OnPropertyChanged();
            }
        }

        private string _stateOriginal;

        public string StateOriginal
        {
            get => _stateOriginal;
            set
            {
                _stateOriginal = value;
                OnPropertyChanged();
            }
        }

        public string StateBinary => ToBinary(StateOriginal);

        private string ToBinary(string hex)
        {
            return String.Join(String.Empty,
                hex.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );
        }

        public void EnableView()
        {
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request({ObjectId}, view)")
            };

            OnCommandsReady(this, ctrlCmds);
        }

        public void DisableView()
        {
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"release({ObjectId}, view)")
            };

            OnCommandsReady(this, ctrlCmds);
        }

        public override void Parse(List<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg == null)
                    continue;

                if (arg.Name.Equals("ports", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Ports = v;
                    else
                        Ports = -1;
                }
            }
        }
    }
}
