using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecos2Core;

namespace Dispatcher
{
    public delegate void BlocksReceivedDelegator(object sender, IReadOnlyList<IBlock> blocks);

    public interface ICommunication
    {
        event BlocksReceivedDelegator BlocksReceived;
        event EventHandler CommunicationStarted;
        event EventHandler CommunicationFailed;

        bool Start();
        Task SendCommand(ICommand command);
        Task SendCommands(IReadOnlyList<ICommand> commands);
    }
}
