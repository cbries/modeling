using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Route : Item
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Type { get; set; }

        public override void Parse(List<CommandArgument> arguments)
        {

        }
    }
}
