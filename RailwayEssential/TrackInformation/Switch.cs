using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Switch : Item
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public List<string> Addrext { get; set; }
        public int Addr { get; set; }
        public string Protocol { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public bool SwitchState { get; set; } // Argument: "switch"

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
                else if (arg.Name.Equals("symbol", StringComparison.OrdinalIgnoreCase))
                {
                    bool v;
                    if (bool.TryParse(arg.Parameter[0], out v))
                        SwitchState = v;
                    else
                        SwitchState = false;
                }
            }
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
