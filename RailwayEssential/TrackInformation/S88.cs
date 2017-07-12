using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class S88 : Item
    {
        public int Ports { get; set; }

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
