using System.Collections.Generic;
using System.Threading.Tasks;
using Ecos2Core;
using TrackInformation;

namespace Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        public RailwayEssentialCore.Configuration Configuration { get; set; }
        public ILogging Logger { get; set; }
        private readonly Communication _communication = null;
        private readonly DataProvider _dataProvider = new DataProvider();

        public DataProvider GetDataProvider()
        {
            return _dataProvider;
        }

        public Dispatcher()
        {
            Configuration = new RailwayEssentialCore.Configuration();

            _communication = new Communication(Configuration);
            _communication.CommunicationStarted += CommunicationOnCommunicationStarted;
            _communication.BlocksReceived += CommunicationOnBlocksReceived;
        }

        public async Task ForwardCommands(IReadOnlyList<ICommand> commands)
        {
            await _communication.SendCommands(commands);
        }

        public async void SetRunMode(bool state)
        {
            if (state)
            {
                _communication.Cfg = Configuration;
                _communication.Logger = Logger;
                _communication.Start();
            }
            else
            {
                await UnloadViews();

                _communication.Shutdown();
            }
        }

        private async Task UnloadViews()
        {
            if (_dataProvider != null)
            {
                foreach (var data in _dataProvider.Objects)
                    data?.DisableView();
            }

            List<ICommand> initialCommands = new List<ICommand>
            {
                CommandFactory.Create("release(1, view)"),
                CommandFactory.Create("release(26, view)"),
                CommandFactory.Create("release(5, view)"),
                CommandFactory.Create("release(100, view)"),
                CommandFactory.Create("release(10, view)"),
                CommandFactory.Create("release(11, view)"),
            };

            await _communication.SendCommands(initialCommands);
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
                CommandFactory.Create("queryObjects(26, ports)"),
                CommandFactory.Create("get(1, info, status)")
            };

            _communication.SendCommands(initialCommands);
        }

        private void CommunicationOnBlocksReceived(object sender, IReadOnlyList<IBlock> blocks)
        {
            if (Logger != null)
                Logger.Log("<Dispatcher> Blocks received: " + blocks.Count);

            foreach (var blk in blocks)
            {
                if (blk == null)
                    continue;

                if(_dataProvider != null)
                    _dataProvider.Add(blk);
            }
        }
    }
}
