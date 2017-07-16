using System.Collections.Generic;
using System.Threading.Tasks;
using Ecos2Core;
using TrackInformation;

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

        public void InitializeWeaving(TrackPlanParser.Track track)
        {
            _track = track;

            _trackWeaver = new TrackWeaver.TrackWeaver();
            var s88_0 = _dataProvider.GetObjectBy(100) as S88;
            var s88_1 = _dataProvider.GetObjectBy(101) as S88;
            var s88_2 = _dataProvider.GetObjectBy(102) as S88;
            var s88_3 = _dataProvider.GetObjectBy(103) as S88;

            var trackObject_A = _track.Get(17, 1);
            var trackObject_B = _track.Get(17, 2);
            var trackObject_C = _track.Get(17, 3);
            var trackObject_D = _track.Get(17, 4);
            var trackObject_E = _track.Get(17, 5);
            var trackObject_F = _track.Get(17, 6);
            var trackObject_G = _track.Get(17, 7);
            var trackObject_H = _track.Get(17, 8);

            _trackWeaver.Link(s88_0, trackObject_A, () => s88_0 != null && s88_0.Pin(9));
            _trackWeaver.Link(s88_0, trackObject_B, () => s88_0 != null && s88_0.Pin(10));
            _trackWeaver.Link(s88_0, trackObject_C, () => s88_0 != null && s88_0.Pin(11));
            _trackWeaver.Link(s88_0, trackObject_D, () => s88_0 != null && s88_0.Pin(12));
            _trackWeaver.Link(s88_0, trackObject_E, () => s88_0 != null && s88_0.Pin(13));
            _trackWeaver.Link(s88_0, trackObject_F, () => s88_0 != null && s88_0.Pin(14));
            _trackWeaver.Link(s88_0, trackObject_G, () => s88_0 != null && s88_0.Pin(15));
            _trackWeaver.Link(s88_0, trackObject_H, () => s88_0 != null && s88_0.Pin(16));

            var trackObject_AA = _track.Get(7, 1);
            var trackObject_BB = _track.Get(7, 2);
            var trackObject_CC = _track.Get(7, 3);
            var trackObject_DD = _track.Get(7, 4);
            var trackObject_EE = _track.Get(7, 5);
            var trackObject_FF = _track.Get(7, 6);
            var trackObject_GG = _track.Get(7, 8);
            var trackObject_HH = _track.Get(7, 7);

            _trackWeaver.Link(s88_1, trackObject_AA, () => s88_1 != null && s88_1.Pin(9));
            _trackWeaver.Link(s88_1, trackObject_BB, () => s88_1 != null && s88_1.Pin(10));
            _trackWeaver.Link(s88_1, trackObject_CC, () => s88_1 != null && s88_1.Pin(11));
            _trackWeaver.Link(s88_1, trackObject_DD, () => s88_1 != null && s88_1.Pin(12));
            _trackWeaver.Link(s88_1, trackObject_EE, () => s88_1 != null && s88_1.Pin(13));
            _trackWeaver.Link(s88_1, trackObject_FF, () => s88_1 != null && s88_1.Pin(14));
            _trackWeaver.Link(s88_1, trackObject_GG, () => s88_1 != null && s88_1.Pin(15));
            _trackWeaver.Link(s88_1, trackObject_HH, () => s88_1 != null && s88_1.Pin(16));
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
                //CommandFactory.Create("request(100, view)"),
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
