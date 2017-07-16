using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ecos2Core;
using RailwayEssentialCore;
using TrackInformation;
using TrackWeaver;

namespace Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        public event UpdateUiDelegate UpdateUi;

        public RailwayEssentialCore.Configuration Configuration { get; set; }
        public ILogging Logger { get; set; }
        private readonly Communication _communication = null;
        private readonly DataProvider _dataProvider = new DataProvider();
        private TrackWeaver.TrackWeaver _trackWeaver;
        private TrackPlanParser.Track _track;

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

        public bool InitializeWeaving(TrackPlanParser.Track track)
        {
            _track = track;

            _trackWeaver = new TrackWeaver.TrackWeaver();

            var filepath = $@"\Sessions\0\TrackWeaving.json".ExpandRailwayEssential();
            if (!File.Exists(filepath))
                return false;
            
            TrackWeaveItems weaverItems = new TrackWeaveItems();
            if (!weaverItems.Load(filepath))
                return false;

            foreach (var item in weaverItems.Items)
            {
                if (item == null)
                    continue;

                switch (item.Type)
                {
                    case WeaveItemT.S88:
                    {
                        var s88item = _dataProvider.GetObjectBy(item.ObjectId) as S88;

                        if (s88item != null)
                        {
                            var trackObject = _track.Get(item.VisuX, item.VisuY);

                            if (trackObject != null)
                            {
                                _trackWeaver.Link(s88item, trackObject, 
                                    () => s88item != null && s88item.Pin((uint)item.Pin));
                                }
                        }
                    }
                        break;

                    // TODO add other item weaves :-D
                }
            }
            
            return true;
        }

        public async Task ForwardCommands(IReadOnlyList<ICommand> commands)
        {
            if (!_communication.IsConnected)
            {
                if (Logger != null)
                {
                    foreach(var cmd in commands)
                        Logger.Log($"<DryRun> " + cmd);
                }
            }
            else
            {
                await _communication.SendCommands(commands);
            }
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

            if (UpdateUi != null)
                UpdateUi(this, _trackWeaver);
        }
    }
}
