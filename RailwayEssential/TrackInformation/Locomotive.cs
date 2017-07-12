using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class Locomotive : Item
    {
        public string Name { get; set; }
        public string Protocol { get; set; }

        public void ToggleFunction(uint nr, bool state)
        {
            int v = state ? 1 : 0;
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request({ObjectId}, control, force)"),
                CommandFactory.Create($"set({ObjectId}, func[{nr}, {v}])"),
                CommandFactory.Create($"release({ObjectId}, control)")
            };

            OnCommandsReady(this, ctrlCmds);
        }

        public void ChangeSpeed(int percentage)
        {
            
        }

        public override void Parse(List<CommandArgument> arguments)
        {
        }
    }
}
