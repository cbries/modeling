using System.Collections.Generic;
using Ecos2Core;
using TrackInformation;

namespace Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        public RailwayEssentialCore.Configuration Configuration { get; set; }
        private readonly Communication _communication = null;
        private readonly DataProvider _dataProvider = new DataProvider();

        public Dispatcher()
        {
            Configuration = new RailwayEssentialCore.Configuration();

            _communication = new Communication(Configuration);
            _communication.CommunicationStarted += CommunicationOnCommunicationStarted;
            _communication.BlocksReceived += CommunicationOnBlocksReceived;
        }

        private void CommunicationOnCommunicationStarted(object sender)
        {
            // send initial commands
            List<ICommand> initialCommands = new List<ICommand>()
            {
                CommandFactory.Create("request(1, view)"),
                CommandFactory.Create("request(26, view)"),
                CommandFactory.Create("request(5, view)"),
                CommandFactory.Create("request(100, view)"),
                CommandFactory.Create("request(10, view)"),
                CommandFactory.Create("request(11, view, viewswitch)"),
                CommandFactory.Create("queryObjects(11, addr, protocol, type, addrext, mode, symbol, name1, name2, name3)"),
                CommandFactory.Create("queryObjects(10, addr, name, protocol)"),
                CommandFactory.Create("queryObjects(26, ports)")
            };

            _communication.SendCommands(initialCommands);
        }

        private void CommunicationOnBlocksReceived(object sender, IReadOnlyList<IBlock> blocks)
        {
            foreach (var blk in blocks)
            {
                if (blk == null)
                    continue;

                _dataProvider.Add(blk);
            }
        }
    }
}
