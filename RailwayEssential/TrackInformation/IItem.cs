using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public interface IItem
    {
        int ObjectId { get; set; }

        void Parse(List<CommandArgument> arguments);
    }
}
