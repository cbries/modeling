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
            
        }
    }
}
