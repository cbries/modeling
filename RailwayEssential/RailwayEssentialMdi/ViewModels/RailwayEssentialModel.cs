using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ecos2Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;
using RailwayEssentialMdi.Bases;
using RailwayEssentialMdi.Commands;
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

        public IMainView MainView { get; set; }

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

        private readonly Theme.Theme _theme;
        private readonly Configuration _cfg;
        private Dispatcher.Dispatcher _dispatcher;

        private readonly SynchronizationContext _ctx = null;

        public Dispatcher.Dispatcher Dispatcher => _dispatcher;

        private readonly Category _itemStatus = new Category { Title = "Status", IconName = "cat_status.png" };
        private readonly Category _itemLocomotives = new Category { Title = "Locomotives", IconName = "cat_locomotive.png" };
        private readonly Category _itemS88 = new Category { Title = "S88 Ports", IconName = "cat_s88.png" };
        private readonly Category _itemSwitches = new Category { Title = "Switches", IconName = "cat_switch.png" };
        private readonly Category _itemRoutes = new Category { Title = "Routes", IconName = "cat_route.png" };

        private ObservableCollection<Item> _rootItems = new ObservableCollection<Item>();

        public ObservableCollection<Item> RootItems
        {
            get => _rootItems;
            set
            {
                _rootItems = value;
                RaisePropertyChanged("RootItems");
            }
        }

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

        public ImageSource ConnectionStateIcon
        {
            get
            {
                if (_dispatcher == null || _cfg == null)
                    return new BitmapImage(new Uri("/RailwayEssentialMdi;component/Resources/offline.png", UriKind.Relative));
                if (!_dispatcher.GetRunMode())
                    return new BitmapImage(new Uri("/RailwayEssentialMdi;component/Resources/offline.png", UriKind.Relative));
                return new BitmapImage(new Uri("/RailwayEssentialMdi;component/Resources/online.png", UriKind.Relative));
            }
        }

        public string ConnectionState
        {
            get
            {
                if (_dispatcher == null || _cfg == null)
                    return "No connection";
                if (!_dispatcher.GetRunMode())
                    return "No connection";
                return $"{_cfg.IpAddress}:{_cfg.Port}";
            }
        }

        public RelayCommand NewProjectCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand ExitCommand { get; }

        public RelayCommand ShowLocomotiveCommand { get; }
        public RelayCommand ConnectCommand { get; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand CmdStationsPropertiesCommand { get; }

        public RelayCommand ShowLogCommand { get; }
        public RelayCommand ShowCommandLogCommand { get; }

        public RelayCommand AddTrackCommand { get; }
        public RelayCommand RemoveTrackCommand { get; }

        private readonly LogEntity _logMessagesGeneral = new LogEntity();
        private readonly LogEntity _logMessagesCommands = new LogEntity();
        private TrackEntity _trackEntity = null;

        public RailwayEssentialModel()
        {
            Windows = new ObservableCollection<IContent>();

            _ctx = SynchronizationContext.Current;

            _cfg = new Configuration();
            
            NewProjectCommand = new RelayCommand(NewProject, CheckNewProject);
            OpenCommand = new RelayCommand(Open, CheckOpen);
            CloseCommand = new RelayCommand(Close, CheckClose);
            SaveCommand = new RelayCommand(Save, CheckSave);
            ExitCommand = new RelayCommand(Exit, CheckExit);
            ShowLocomotiveCommand = new RelayCommand(ShowLocomotive, CheckShowLocomotive);
            ConnectCommand = new RelayCommand(ConnectToCommandStation, CheckConnectToCommandStation);
            DisconnectCommand = new RelayCommand(DisconnectFromCommandStation, CheckDisconnectFromCommandStation);
            CmdStationsPropertiesCommand = new RelayCommand(PropertiesCommandStation);
            ShowLogCommand = new RelayCommand(ShowLog);
            ShowCommandLogCommand = new RelayCommand(ShowCommandLog);
            AddTrackCommand = new RelayCommand(AddTrack, CheckAddTrack);
            RemoveTrackCommand = new RelayCommand(RemoveTrack, CheckRemoveTrack);

            // TEST
            //int _counter = 0;
            //new Thread(() =>
            //{
            //    for (;;)
            //    {
            //        if (_logMessagesGeneral != null)
            //        {
            //            _logMessagesGeneral.Add("Message: {0}\r\n", _counter);

            //            if (_itemStatus != null)
            //            {
            //                if (_ctx != null)
            //                {
            //                    _ctx.Send(state =>
            //                    {
            //                        // ...
            //                    }, null);
            //                }
            //            }

            //            ++_counter;

            //            Thread.Sleep(1000);
            //        }
            //    }
            //})
            //{ IsBackground = true }.Start();

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
                        ["objects"] = new JArray() { "TrackObjects.json" },
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

            RootItems.Add(_itemStatus);
            RootItems.Add(_itemLocomotives);
            RootItems.Add(_itemS88);
            RootItems.Add(_itemSwitches);
            RootItems.Add(_itemRoutes);

            _cfg.IpAddress = Project.TargetHost;
            _cfg.Port = Project.TargetPort;

            _dispatcher = new Dispatcher.Dispatcher(_cfg)
            {
                Configuration = _cfg,
                Model = this,
                Logger = this
            };

            var prjTrack = Project.Track;

            _trackEntity = new TrackEntity(_dispatcher)
            {
                TrackObjectFilepath = Path.Combine(_project.Dirpath, prjTrack.Path),
                Theme = _theme,
                Ctx = _ctx,
                ProjectTrack = prjTrack
            };

            _trackEntity.Initialize();

            if (prjTrack.Show)
            {
                var item = new TrackWindow(_trackEntity);
                item.Closing += (s, ev) => Windows.Remove(item);
                Windows.Add(item);
            }

            _dispatcher.UpdateUi += DispatcherOnUpdateUi;
            _dispatcher.ReadyToPlay += DispatcherOnReadyToPlay;

            var dataProvider = _dispatcher.GetDataProvider();
            dataProvider.DataChanged += OnDataChanged;
            dataProvider.CommandsReady += DataProviderOnCommandsReady;

            foreach (var objFilename in Project.Objects)
            {
                string absolutePath = Path.Combine(Project.Dirpath, objFilename);
                if (!File.Exists(absolutePath))
                    continue;

                dataProvider.LoadObjects(absolutePath);
            }

            //if (MainView != null)
            //    MainView.LoadLayout();
        }

        private T GetWindow<T>() where T : class
        {
            foreach (var item in Windows)
            {
                if (item is T)
                    return item as T;
            }

            return default(T);
        }

        private void DispatcherOnReadyToPlay(object sender, EventArgs eventArgs)
        {
            var weaveFilepath = Path.Combine(Project.Dirpath, Project.Track.Weave);
            _dispatcher.InitializeWeaving(_trackEntity.Track, weaveFilepath);
        }

        private void DispatcherOnUpdateUi(object sender, TrackWeaver.TrackWeaver trackWeaver)
        {
            foreach (var w in Windows)
            {
                var ww = w as TrackWindow;
                if (ww == null || ww.Entity == null)
                    continue;

                ww.Entity.UpdateTrackViewerUi(trackWeaver);
            }
        }

        private void DataProviderOnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
            if (_dispatcher != null)
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

                        if (_itemStatus != null && _itemStatus.Items.Count < 4)
                        {
                            _itemStatus.Items.Clear();
                            _itemStatus.Items.Add(new Item { Title = $"{ee.Name}", IconName = "esu0.png" });
                            _itemStatus.Items.Add(new Item { Title = $"Application Version: {ee.ApplicationVersion}", IconName = "esu0.png" });
                            _itemStatus.Items.Add(new Item { Title = $"Protocol Version: {ee.ProtocolVersion}", IconName = "esu0.png" });
                            _itemStatus.Items.Add(new Item { Title = $"Hardware Version: {ee.HardwareVersion}", IconName = "esu0.png" });
                        }
                        else
                        {
                            if (_itemStatus != null)
                            {
                                _itemStatus.Items[0].Title = $"{ee.Name}";
                                _itemStatus.Items[1].Title = $"Application Version: {ee.ApplicationVersion}";
                                _itemStatus.Items[2].Title = $"Protocol Version: {ee.ProtocolVersion}";
                                _itemStatus.Items[3].Title = $"Hardware Version: {ee.HardwareVersion}";
                            }
                        }
                    }
                    else if (e is Locomotive)
                    {
                        var ee = e as Locomotive;

                        if (_itemLocomotives.Items.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _itemLocomotives.Items.Add(ee);
                        }

                        Log($"Locomotive {ee.Addr}, {ee.Name}");
                    }
                    else if (e is S88)
                    {
                        var ee = e as S88;

                        if (_itemS88.Items.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _itemS88.Items.Add(ee);
                        }
                    }
                    else if (e is Switch)
                    {
                        var ee = e as Switch;

                        if (_itemSwitches.Items.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _itemSwitches.Items.Add(ee);
                        }
                    }
                    else if (e is Route)
                    {
                        var ee = e as Route;

                        if (_itemRoutes.Items.Any(x => x.ObjectId == ee.ObjectId))
                            ee.UpdateTitle();
                        else
                        {
                            ee.UpdateTitle();
                            _itemRoutes.Items.Add(ee);
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

                RootItems.Clear();

                Project = null;
            }
        }

        public void Save(object p)
        {
            var ee = _trackEntity as IPersist;
            if(ee != null)
            { 
                bool r = ee.Save();
                if (!r)
                    Log("<Save> Failure storing file: " + _trackEntity.TrackObjectFilepath);
            }

            Project.TargetHost = _cfg.IpAddress;
            Project.TargetPort = _cfg.Port;

            // transfer window dimensions


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

        public void ShowLocomotive(object p)
        {
            var w = GetWindow<LocomotivesWindow>();
            if (w != null)
            {
                w.Entity.ObjectItem = _currentLocomotive;

                return;
            }

            var item2 = new LocomotivesWindow();
            item2.Entity = new LocomotiveEntity {ObjectItem = _currentLocomotive};
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
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
                LogNetwork("Could not connect to command station ({1}:{2}): {0}\r\n", ex.Message, _cfg.IpAddress, _cfg.Port);
            }
        }

        public void DisconnectFromCommandStation(object p)
        {
            if (_dispatcher != null)
                _dispatcher.SetRunMode(false);
        }

        public void PropertiesCommandStation(object p)
        {
            var w = GetWindow<PropertiesWindow>();
            if (w != null)
                return;

            var item2 = new PropertiesWindow(_cfg);
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void ShowLog(object p)
        {
            var w = GetWindow<LogWindow>();
            if (w?.LogMode == LogWindow.Mode.General)
                return;

            var item2 = new LogWindow(_logMessagesGeneral) { LogMode = LogWindow.Mode.General };
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void ShowCommandLog(object p)
        {
            var w = GetWindow<LogWindow>();
            if (w?.LogMode == LogWindow.Mode.Commands)
                return;

            var item2 = new LogWindow(_logMessagesCommands) { LogMode = LogWindow.Mode.Commands };
            item2.Closing += (s, e) => Windows.Remove(item2);
            Windows.Add(item2);
        }

        public void AddTrack(object p)
        {
            var trackPath = "TrackPlan{0}.json".GenerateUniqueName(_project.Dirpath);
            var trackWeavePath = "TrackWeave{0}.json".GenerateUniqueName(_project.Dirpath);
            try { File.WriteAllText(trackPath, @"[]", Encoding.UTF8); }
            catch { /* ignore */ }
            try { File.WriteAllText(trackWeavePath, @"[]", Encoding.UTF8); }
            catch { /* ignore */ }

            var trackName = Path.GetFileNameWithoutExtension(trackPath);
            var trackRelativePath = Path.GetFileName(trackPath);
            var trackWeaveRelativePath = Path.GetFileName(trackWeavePath);

            if (_trackEntity == null)
            {
                _trackEntity = new TrackEntity(_dispatcher)
                {
                    Theme = _theme,
                    Ctx = _ctx,
                    TrackObjectFilepath = trackPath
                };

                _trackEntity.Initialize();
            }

            var item = new TrackWindow(_trackEntity);
            item.Closing += (s, ev) => Windows.Remove(item);
            Windows.Add(item);

            Project.Track = new ProjectTrack
            {
                Name = trackName,
                Path = trackRelativePath,
                Show = true,
                Weave = trackWeaveRelativePath
            };
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

        private bool CheckShowLocomotive(object o1)
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

        private TrackInformation.Locomotive _currentLocomotive;
        private TrackInformation.Switch _currentSwitch;

        public void SetCurrentLocomotive(object locomotiveItem)
        {
            _currentLocomotive = locomotiveItem as TrackInformation.Locomotive;
            ShowLocomotive(null);
        }

        public void SetCurrentSwitch(object switchItem)
        {
            _currentSwitch = switchItem as TrackInformation.Switch;
            // TODO add ShowSwitch();
        }

        #endregion        
    }
}
