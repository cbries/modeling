using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Locomotive : IItem
    {
        public int ObjectId { get; set; }

        public string Name { get; set; }
        public int Address { get; set; }
        public string Protocol { get; set; }

        public void Parse(List<CommandArgument> arguments)
        {

        }

    }
}
