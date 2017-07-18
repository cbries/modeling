using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ecos2Core;
using Newtonsoft.Json.Linq;

namespace TrackInformation
{
    public class Locomotive : Item
    {
        public override int TypeId() { return 1; }

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

        // MM14, MM27, MM28, DCC14, DCC28, DCC128, SX32, MMFKT
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

        private int _speed; // percentage

        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged();
            }
        }
        
        private int _speedstep;

        public int Speedstep
        {
            get => _speedstep;
            set
            {
                _speedstep = value;
                OnPropertyChanged();
            }
        }

        private int _directon;

        public int Direction
        {
            get => _directon;
            set
            {
                _directon = value;
                OnPropertyChanged();
            }
        }

        private string _funcset;

        public string Funcset
        {
            get => _funcset;
            set
            {
                _funcset = value;
                OnPropertyChanged();
            }
        }

        public Locomotive() : base()
        {
        }

        public override void UpdateTitle()
        {
            string v = Direction == 1 ? "Backward" : "Forward";
            
            Title = $"{ObjectId} {Name} Speed[{Speed}]->{v} ({Protocol} : {Addr})";
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

        public void QueryState()
        {
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"get({ObjectId}, speed, profile, protocol, name, addr, dir, funcset)"),
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
                else if (arg.Name.Equals("speed", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Speed = v;
                    else
                        Speed = -1;
                }
                else if (arg.Name.Equals("speedstep", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Speedstep = v;
                    else
                        Speedstep = -1;
                }
                else if (arg.Name.Equals("dir", StringComparison.OrdinalIgnoreCase))
                {
                    int v;
                    if (int.TryParse(arg.Parameter[0], out v))
                        Direction = v;
                    else
                        Direction = -1;
                }
                else if (arg.Name.Equals("funcset", StringComparison.OrdinalIgnoreCase))
                {
                    Funcset = arg.Parameter[0].Trim();
                }
                else if (arg.Name.Equals("func", StringComparison.OrdinalIgnoreCase))
                {
                    Trace.WriteLine($"Func {arg.Parameter[0]} to {arg.Parameter[1]}");
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
            JObject o = new JObject
            {
                ["objectId"] = ObjectId,
                ["name"] = _name,
                ["protocol"] = _protocol,
                ["addr"] = _addr,
                ["speed"] = _speed,
                ["speedstep"] = _speedstep,
                ["direction"] = _directon,
                ["funcset"] = _funcset
            };

            return o;
        }

        public override void ParseJson(JObject obj)
        {
            if (obj == null)
                return;

            if (obj["objectId"] != null)
                ObjectId = (int)obj["objectId"];
            if (obj["name"] != null)
                Name = obj["name"].ToString();
            if (obj["protocol"] != null)
                Protocol = obj["protocol"].ToString();
            if (obj["addr"] != null)
                Addr = (int) obj["addr"];
            if (obj["speed"] != null)
                Speed = (int) obj["speed"];
            if (obj["speedstep"] != null)
                Speedstep = (int) obj["speedstep"];
            if (obj["direction"] != null)
                Direction = (int) obj["direction"];
            if (obj["funcset"] != null)
                Funcset = obj["funcset"].ToString();
        }
    }
}
