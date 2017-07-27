using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Ecos2Core;
using MDIContainer.DemoClient.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;
using RailwayEssentialMdi.Bases;
using RailwayEssentialMdi.DataObjects;
using RailwayEssentialMdi.Entities;
using RailwayEssentialMdi.Interfaces;
using TrackInformation;
using Switch = TrackInformation.Switch;

namespace RailwayEssentialMdi.ViewModels
{
    public class RailwayEssentialModel : ViewModelBase, IRailwayEssentialModel, ILogging
    {
        public ObservableCollection<IContent> Windows { get; private set; }

        private ObservableCollection<Item> _commandStations;
        private ObservableCollection<Locomotive> _locomotives;
        private ObservableCollection<Switch> _switches;
        private ObservableCollection<S88> _s88s;
        private ObservableCollection<Route> _routes;

        public ObservableCollection<Item> CommandStations => _commandStations;
        public ObservableCollection<Locomotive> Locomotices => _locomotives;
        public ObservableCollection<Switch> Switches => _switches;
        public ObservableCollection<S88> S88 => _s88s;
        public ObservableCollection<Route> Routes => _routes;

        private Random _random = new Random(DateTime.Now.Millisecond);
        private ProjectFile _project;

        public ProjectFile Project
        {
            get => _project;

            set
            {
                _project = value;
                RaisePropertyChanged("Project");
            }
        }

        private Theme.Theme _theme;
        private Configuration _cfg;
        private Dispatcher.Dispatcher _dispatcher;

        private SynchronizationContext _ctx = null;

        public Dispatcher.Dispatcher Dispatcher => _dispatcher;

        public void Log(string text, params object[] args)
        {
            _logMessagesGeneral?.Add(text, args);
        }

        public void LogNetwork(string text, params object[] args)
        {
            _logMessagesCommands?.Add(text, args);
        }

        private bool _isDirty;

        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                _isDirty = value;
                RaisePropertyChanged("IsDirty");
                RaisePropertyChanged("IsDirtyMessage");
            }
        }

        public string IsDirtyMessage
        {
            get
            {
                if (IsDirty)
                    return "*";
                return "";
            }
        }

        public string ConnectionState
        {
            get
            {
                if (_dispatcher == null)
                    return "No connection";
                if (_cfg == null)
                    return "No connection";
                if (_dispatcher.GetRunMode())
                    return $"{_cfg.IpAddress}:{_cfg.Port}";
                return "No connection";
            }
        }

        public RelayCommand NewProjectCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand ExitCommand { get; }

        public RelayCommand ConnectCommand { get; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand CmdStationsPropertiesCommand { get; }

        public RelayCommand ShowLogCommand { get; }
        public RelayCommand ShowCommandLogCommand { get; }
        public RelayCommand AddTrackCommand { get; }
        public RelayCommand RemoveTrackCommand { get; }

        private readonly LogEntity _logMessagesGeneral = new LogEntity();
        private readonly LogEntity _logMessagesCommands = new LogEntity();

        private readonly List<TrackEntity> _trackEntities = new List<TrackEntity>();

        private static int _counter = 0;

        public RailwayEssentialModel()
        {
            Windows = new ObservableCollection<IContent>();

            _ctx = SynchronizationContext.Current;

            _commandStations = new ObservableCollection<Item>();
            _locomotives = new ObservableCollection<Locomotive>();
            _switches = new ObservableCollection<Switch>();
            _s88s = new ObservableCollection<S88>();
            _routes = new ObservableCollection<Route>();

            _cfg = new Configuration();

            NewProjectCommand = new RelayCommand(NewProject, CheckNewProject);
            OpenCommand = new RelayCommand(Open, CheckOpen);
            CloseCommand = new RelayCommand(Close, CheckClose);
            SaveCommand = new RelayCommand(Save, CheckSave);
            ExitCommand = new RelayCommand(Exit, CheckExit);
            ConnectCommand = new RelayCommand(ConnectToCommandStation, CheckConnectToCommandStation);
            DisconnectCommand = new RelayCommand(DisconnectFromCommandStation, CheckDisconnectFromCommandStation);
            CmdStationsPropertiesCommand = new RelayCommand(PropertiesCommandStation);
            ShowLogCommand = new RelayCommand(ShowLog);
            ShowCommandLogCommand = new RelayCommand(ShowCommandLog);
            AddTrackCommand = new RelayCommand(AddTrack, CheckAddTrack);
            RemoveTrackCommand = new RelayCommand(RemoveTrack, CheckRemoveTrack);

            // TEST
            //new Thread(() =>
            //{
            //    for (;;)
            //    {
            //        if (_logMessagesGeneral != null)
            //        {
            //            _logMessagesGeneral.Add("Message: {0}\r\n", _counter);

            //            ++_counter;

            //            Thread.Sleep(1000);
            //        }
            //    }
            //}) { IsBackground = true }.Start();
            //_logMessagesGeneral.Add("Test\r\n");

            _theme = new Theme.Theme();
            var themePath = Utils.ThemeName.ExpandRailwayEssential();
            if (!themePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                themePath += ".json";
            if (!_theme.Load(themePath))
            {
                Trace.WriteLine("<Theme> Loading of theme failed: " + themePath);
                Log("<Theme> Loading of theme failed: " + themePath);
            }
        }

        public void NewProject(object p)
        {
            var tmpPrjName = "Project{0}\\Project{0}.railwayprj".GenerateUniqueName("Projects\\".ExpandRailwayEssential());

            var dname = Path.GetDirectoryName(tmpPrjName);
            if (!Directory.Exists(dname))
                dname = Path.GetDirectoryName(dname);

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = Path.GetFileName(tmpPrjName),
                InitialDirectory = dname,
                DefaultExt = ".railwayprj",
                Filter = "RailwayEssential (.railwayprj)|*.railwayprj"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                var fname = dlg.FileName;
                var dirname = Path.GetDirectoryName(fname);
                var name = Path.GetFileNameWithoutExtension(fname);

                try
                {
                    Directory.CreateDirectory(Path.Combine(dirname, name));
                    fname = Path.Combine(dirname, name);
                    fname = Path.Combine(fname, name + ".railwayprj");
                    var o = new JObject
                    {
                        ["name"] = name,
                        ["version"] = 1.0,
                        ["targetHost"] = "192.168.178.61",
                        ["targetPort"] = 15471,
                        ["objects"] = new JArray() {"TrackObjects.json"},
                        ["tracks"] = new JArray()
                    };

                    File.WriteAllText(
                        Path.Combine(Path.GetDirectoryName(fname), "TrackObjects.json"), 
                        new JObject().ToString(Formatting.Indented), Encoding.UTF8);

                    File.WriteAllText(fname, o.ToString(Formatting.Indented), Encoding.UTF8);

                    Project = new ProjectFile();
                    Project.Load(fname);
                }
                catch
                {
                    // ignore
                }
            }
        }

        public void Open(object p)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Testprojekte\");
            dlg.DefaultExt = ".railwayprj";
            dlg.Filter = "RailwayEssential Project (.railwayprj)|*.railwayprj";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                try
                {
                    if (Project != null)
                    {
                        if (CloseCommand.CanExecute(null))
                            CloseCommand.Execute(null);
                    }

                    var prj = new ProjectFile();
                    if (!prj.Load(filename))
                        Log("Project load failed: " + prj.Filepath + "\r\n");
                    else
                    {
                        Project = prj;
                        Log("Project opened: " + prj.Name + "\r\n");
                    }
                }
                catch
                {
                    // ignore
                }
            }

            AfterOpen();
        }

        private void AfterOpen()
        {
            if (_project == null)
                return;

            // load tracks
            foreach (var prjTrack in _project.Tracks)
            {
                if (prjTrack == null)
                    continue;
                
                // ...

                TrackEntity e = new TrackEntity(_dispatcher)
                {
                    TrackObjectFilepath = Path.Combine(_project.Dirpath, prjTrack.Path),
                    Theme = _theme, 
                    Ctx = _ctx
                };

                e.Initialize();

                _trackEntities.Add(e);

                if (prjTrack.Show)
                {
                    var item = new TrackWindow(e);
                    item.Closing += (s, ev) => Windows.Remove(item);
                    Windows.Add(item);
                }
            }

            _cfg.IpAddress = Project.TargetHost;
            _cfg.Port = Project.TargetPort;

            _dispatcher = new Dispatcher.Dispatcher(_cfg)
            {
                Configuration = _cfg,
                Model = this,
                Logger = this
            };
            _dispatcher.UpdateUi += DispatcherOnUpdateUi;

            var dataProvider = _dispatcher.GetDataProvider();
            dataProvider.DataChanged += OnDataChanged;
            dataProvider.CommandsReady += DataProviderOnCommandsReady;
        }

        private void DispatcherOnUpdateUi(object sender, TrackWeaver.TrackWeaver trackWeaver)
        {
            foreach (var w in Windows)
            {
                var ww = w as TrackEntity;
                if (ww == null)
                    continue;

                ww.UpdateTrackViewerUi(trackWeaver);
            }
        }

        private void DataProviderOnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
            if(_dispatcher != null)
                _dispatcher.ForwardCommands(commands);
        }

        private void OnDataChanged(object sender)
        {
            _ctx.Send(state =>
            {
                var dataProvider = _dispatcher.GetDataProvider();

                foreach (var e in dataProvider.Objects)
                {
                    if (e == null)
                        continue;

                    if (e is Ecos2)
                    {
                        var ee = e as Ecos2;

                        if (_commandStations.Count == 0)
                            _commandStations.Add(new Ecos2());

                        var ecos2 = _commandStations[0] as Ecos2;

                        if (ecos2 != null && ecos2.Items.Count < 4)
                        {
                            ecos2.Items.Clear();
                            ecos2.Items.Add(new Item {Title = $"{ee.Name}"});
                            ecos2.Items.Add(new Item {Title = $"Application Version: {ee.ApplicationVersion}"});
                            ecos2.Items.Add(new Item {Title = $"Protocol Version: {ee.ProtocolVersion}"});
                            ecos2.Items.Add(new Item {Title = $"Hardware Version: {ee.HardwareVersion}"});
                        }
                        else
                        {
                            if (ecos2 != null)
                            {
                                ecos2.Items[0].Title = $"{ee.Name}";
                                ecos2.Items[1].Title = $"Application Version: {ee.ApplicationVersion}";
                                ecos2.Items[2].Title = $"Protocol Version: {ee.ProtocolVersion}";
                                ecos2.Items[3].Title = $"Hardware Version: {ee.HardwareVersion}";
                            }
                        }
                    }
                    else if (e is Locomotive)
                    {
                        var ee = e as Locomotive;

                        if (_locomotives.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _locomotives.Add(ee);
                        }
                    }
                    else if (e is S88)
                    {
                        var ee = e as S88;

                        if (_s88s.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _s88s.Add(ee);
                        }
                    }
                    else if (e is Switch)
                    {
                        var ee = e as Switch;

                        if (_switches.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _switches.Add(ee);
                        }
                    }
                    else if (e is Route)
                    {
                        var ee = e as Route;

                        if (_routes.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _routes.Add(ee);
                        }
                    }
                }
            }, null);
        }
        
        public void Close(object o)
        {
            if (_project != null)
            {
                if (IsDirty)
                {
                    Save(null);
                }

                Windows.Clear();
                Windows = null;
                Windows = new ObservableCollection<IContent>();

                Project = null;
            }
        }

        public void Save(object p)
        {            
            foreach (var e in _trackEntities)
            {
                var ee = e as IPersist;
                if (ee == null)
                    continue;

                bool r = ee.Save();
                if (!r)
                    Log("<Save> Failure storing file: " + e.TrackObjectFilepath);
            }

            Project.TargetHost = _cfg.IpAddress;
            Project.TargetPort = _cfg.Port;

            Project.Save();

            var globalFilepath = Path.Combine(_project.Dirpath, "TrackObjects.json");
            var r3 = _dispatcher?.GetDataProvider().SaveObjects(globalFilepath);
            if (r3.HasValue)
                Log("Storing failed: " + globalFilepath);
        }

        public void Exit(object p)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void ConnectToCommandStation(object p)
        {
            try
            {
                if (_dispatcher != null)
                    _dispatcher.SetRunMode(true);
            }
            catch (Exception ex)
            {
                LogNetwork("Could not connect to command station ({1}:{2}): {0}", ex.Message, _cfg.IpAddress, _cfg.Port);
            }
        }

        public void DisconnectFromCommandStation(object p)
        {
            if (_dispatcher != null)
                _dispatcher.SetRunMode(false);
        }
     
        public void PropertiesCommandStation(object p)
        {
            foreach (var item in Windows)
            {
                var e = item as PropertiesWindow;
                if (e != null)
                    return;
            }

            var item2 = new PropertiesWindow(_cfg);
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void ShowLog(object p)
        {
            foreach (var item in Windows)
            {
                if (item == null)
                    continue;
                var e = item as LogWindow;
                if (e?.LogMode == LogWindow.Mode.General)
                    return;
            }

            var item2 = new LogWindow(_logMessagesGeneral) { LogMode = LogWindow.Mode.General };
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void ShowCommandLog(object p)
        {
            foreach (var item in Windows)
            {
                if (item == null)
                    continue;
                var e = item as LogWindow;
                if (e?.LogMode == LogWindow.Mode.Commands)
                    return;
            }

            var item2 = new LogWindow(_logMessagesCommands) { LogMode = LogWindow.Mode.Commands };
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void AddTrack(object p)
        {
            var trackPath = "TrackPlan{0}.json".GenerateUniqueName(_project.Dirpath);
            var trackWeavePath = "TrackWeave{0}.json".GenerateUniqueName(_project.Dirpath);
            try { File.WriteAllText("[]", trackPath, Encoding.UTF8); }
            catch { /* ignore */ }
            try { File.WriteAllText("[]", trackWeavePath, Encoding.UTF8); }
            catch { /* ignore */ }

            var trackName = Path.GetFileNameWithoutExtension(trackPath);
            var trackRelativePath = Path.GetFileName(trackPath);
            var trackWeaveRelativePath = Path.GetFileName(trackWeavePath);

            TrackEntity e = new TrackEntity(_dispatcher)
            {
                Theme = _theme,
                Ctx = _ctx,
                TrackObjectFilepath = trackPath
            };
            e.Initialize();

            _trackEntities.Add(e);
            
            var item = new TrackWindow(e);
            item.Closing += (s, ev) => Windows.Remove(item);
            Windows.Add(item);

            Project.Tracks.Add(new ProjectTrack()
            {
                Name = trackName,
                Path = trackRelativePath,
                Show = true,
                Weave = trackWeaveRelativePath
            });
        }

        public void RemoveTrack(object p)
        {
            // TODO
        }
 
        #region can execute checks

        public bool CheckAddTrack(object p)
        {
            if (_project == null)
                return false;
            return true;
        }

        public bool CheckRemoveTrack(object p)
        {
            return CheckAddTrack(p);
        }

        private bool CheckDisconnectFromCommandStation(object p)
        {
            var m = p as RailwayEssentialModel;

            if (m == null)
                return false;

            if (m._dispatcher == null)
                return false;

            return m._dispatcher.GetRunMode();
        }

        private bool CheckConnectToCommandStation(object o1)
        {
            var m = o1 as RailwayEssentialModel;

            if (m == null)
                return false;

            if (m._project == null)
                return false;

            if (m._cfg == null)
                return false;

            return true;
        }

        public bool CheckNewProject(object p)
        {
            return CheckOpen(p);
        }

        public bool CheckOpen(object p)
        {
            if (_project == null)
                return true;

            return false;
        }

        public bool CheckClose(object p)
        {
            if (_project == null)
                return false;

            return true;
        }

        public bool CheckSave(object p)
        {
            if (_project == null)
                return false;

            return true;
        }

        public bool CheckExit(object p)
        {
            return true;
        }

        #endregion

        #region IRailwayEssentialModel

        public void TriggerPropertyChanged(string name)
        {
            if (_ctx == null || string.IsNullOrEmpty(name))
                return;

            _ctx.Send(state =>
            {
                RaisePropertyChanged(name);
            }, null);
        }

        #endregion
    }
}
