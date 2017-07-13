using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ecos2Core;
using Newtonsoft.Json.Linq;

namespace TrackInformation
{
    public class Switch : Item
    {
        private readonly string[] _names = new string[3];

        public string Name1
        {
            get => _names[0];
            set
            {
                _names[0] = value;
                OnPropertyChanged();
            }
        }

        public string Name2
        {
            get => _names[1];
            set
            {
                _names[1] = value;
                OnPropertyChanged();
            }
        }

        public string Name3
        {
            get => _names[2];
            set
            {
                _names[2] = value;
                OnPropertyChanged();
            }
        }

        private List<string> _addrext = new List<string>();

        public List<string> Addrext
        {
            get => _addrext;
            set
            {
                _addrext = value;
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

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _mode;

        public string Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        private int _state;

        public int State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        private int _switching;

        public int Switching
        {
            get => _switching;
            set
            {
                _switching = value;
                OnPropertyChanged();
            }
        }

        public override void UpdateTitle()
        {
            var ext = string.Join(", ", Addrext);
            var direction = "Turn";
            if (State == 0)
                direction = "Straight";
            else
                direction = "Turn";
            Title = $"{ObjectId} {Name1}[{ext}] {direction}";
        }

        public override void Parse(List<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg == null)
                    continue;

                if (arg.Name.Equals("name1", StringComparison.OrdinalIgnoreCase))
                    Name1 = arg.Parameter[0];
                else if (arg.Name.Equals("name2", StringComparison.OrdinalIgnoreCase))
                    Name2 = arg.Parameter[0];
                else if (arg.Name.Equals("name3", StringComparison.OrdinalIgnoreCase))
                    Name3 = arg.Parameter[0];
                else if (arg.Name.Equals("addrext", StringComparison.OrdinalIgnoreCase))
                    Addrext = arg.Parameter;
                else if (arg.Name.Equals("addr", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Addr = v;
                    else
                        Addr = -1;
                }
                else if (arg.Name.Equals("protocol", StringComparison.OrdinalIgnoreCase))
                    Protocol = arg.Parameter[0];
                else if (arg.Name.Equals("type", StringComparison.OrdinalIgnoreCase))
                    Type = arg.Parameter[0];
                else if (arg.Name.Equals("mode", StringComparison.OrdinalIgnoreCase))
                    Mode = arg.Parameter[0];
                else if (arg.Name.Equals("state", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        State = v;
                    else
                        State = -1;
                }
                else if (arg.Name.Equals("switching", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Switching = v;
                    else
                        Switching = -1;
                }
                else if (arg.Name.Equals("symbol", StringComparison.OrdinalIgnoreCase))
                {
                    Trace.WriteLine($"Handled, but purpose is unknown for: {arg.Name} -> {arg.Parameter[0]}");
                }
                else
                {
                    Trace.WriteLine("Unknown argument: " + arg.Name + " -> " + string.Join(", ", arg.Parameter));
                }
            }

            OnPropertyChanged();
        }

        public override JObject ToJson()
        {
            return null;
        }

        public override void ParseJson(JObject obj)
        {

        }

        public void ChangeDirection(int index)
        {
            string s = Addrext[index];
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request(11, control, force)"),
                CommandFactory.Create($"set(11, switch[{Protocol}{s}])"),
                CommandFactory.Create($"release(11, control)")
            };

            OnCommandsReady(this, ctrlCmds);
        }
    }
}
