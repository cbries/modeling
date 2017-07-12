using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Ecos2 : Item
    {
        public string Name { get { return "ECoS2"; } }
        public string ProtocolVersion { get; set; }
        public string ApplicationVersion { get; set; }
        public string HardwareVersion { get; set; }
        public string Status { get; set; }

        public override void Parse(List<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg == null)
                    continue;

                if (arg.Name.Equals("status", StringComparison.OrdinalIgnoreCase))
                    Status = arg.Parameter[0];
                else if (arg.Name.Equals("ProtocolVersion", StringComparison.OrdinalIgnoreCase))
                    ProtocolVersion = arg.Parameter[0];
                else if (arg.Name.Equals("ApplicationVersion", StringComparison.OrdinalIgnoreCase))
                    ApplicationVersion = arg.Parameter[0];
                else if (arg.Name.Equals("HardwareVersion", StringComparison.OrdinalIgnoreCase))
                    HardwareVersion = arg.Parameter[0];
            }

        }
    }
}
