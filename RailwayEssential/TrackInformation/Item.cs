using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Item : IItem
    {
        public event CommandsReadyDelegator CommandsReady;

        public int ObjectId { get; set; }

        public virtual void Parse(List<CommandArgument> arguments)
        {
        }

        protected virtual void OnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
            if (CommandsReady != null)
                CommandsReady(sender, commands);
        }
    }
}
