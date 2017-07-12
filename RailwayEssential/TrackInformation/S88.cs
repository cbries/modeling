using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class S88 : Item
    {
        public int Ports { get; set; }

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
