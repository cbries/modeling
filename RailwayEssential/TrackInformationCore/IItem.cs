using System.Collections.Generic;
using Ecos2Core;
using Newtonsoft.Json.Linq;

namespace TrackInformationCore
{
    public delegate void CommandsReadyDelegator(object sender, IReadOnlyList<ICommand> commands);

    public interface IItem
    {
        event CommandsReadyDelegator CommandsReady;

        string IconName { get; set; }

        int TypeId();

        int ObjectId { get; set; }

        bool IsRouted { get; set; }

        bool HasView { get; }

        void Parse(List<CommandArgument> arguments);

        void EnableView();
        void DisableView();

        JObject ToJson();
        void ParseJson(JObject obj);
    }
}
