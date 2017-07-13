using System.Collections.Generic;
using Ecos2Core;
using Newtonsoft.Json.Linq;

namespace TrackInformation
{
    public delegate void CommandsReadyDelegator(object sender, IReadOnlyList<ICommand> commands);

    public interface IItem
    {
        event CommandsReadyDelegator CommandsReady;

        int ObjectId { get; set; }

        void Parse(List<CommandArgument> arguments);

        void EnableView();
        void DisableView();

        JObject ToJson();
        void ParseJson(JObject obj);
    }
}
