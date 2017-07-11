using System.Collections.Generic;

namespace Ecos2Core
{
    public interface ICommand
    {
        CommandT Type { get; }
        string Name { get; }
        string NativeCommand { get; set; }
        int ObjectId { get; }
        List<CommandArgument> Arguments { get; set; }

        bool Parse();
    }
}
