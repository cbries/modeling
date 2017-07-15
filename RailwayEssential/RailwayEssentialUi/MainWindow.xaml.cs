using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Ecos2Core;
using Newtonsoft.Json;
using RailwayEssentialCore;
using TrackInformation;

namespace RailwayEssentialUi
{
    public partial class MainWindow : Window, ILogging
    {
        public const int SessionNumber = 0;

        public string TrackObjectFile
        {
            get
            {
                var dirPath = $@"Sessions\{SessionNumber}".ExpandRailwayEssential();
                if (!string.IsNullOrEmpty(dirPath))
                    return Path.Combine(dirPath, "TrackPlan.json");
                return null;
            }
        }

        public string TrackGlobalFile
        {
            get
            {
                var dirPath = $@"Sessions\{SessionNumber}".ExpandRailwayEssential();
                if (!string.IsNullOrEmpty(dirPath))
                    return Path.Combine(dirPath, "TrackObjects.json");
                return null;
            }
        }

        private readonly RailwayEssentialCore.Configuration _cfg;
        private readonly Dispatcher.Dispatcher _dispatcher;

        private SynchronizationContext _ctx = null;

        public void Log(string msg)
        {
            if (_ctx == null)
                return;

            _ctx.Send(state =>
            {
                string currentMsg = TxtLogging.Text;
                TxtLogging.Text = currentMsg.Trim() + "\r\n" + msg.Trim();
                TxtLogging.ScrollToEnd();
            }, null);
        }

        public void LogNetwork(string msg)
        {
            if (_ctx == null)
                return;

            _ctx.Send(state =>
            {
                string currentMsg = TxtLoggingNetwork.Text;
                TxtLoggingNetwork.Text = currentMsg.Trim() + "\r\n" + msg.Trim();
                TxtLoggingNetwork.ScrollToEnd();
            }, null);
        }

        private Locomotive _currentLocomotive = null;
        private Switch _currentSwitch = null;

        public MainWindow()
        {
            InitializeComponent();

            _ctx = SynchronizationContext.Current;
            _cfg = new RailwayEssentialCore.Configuration { IpAddress = "192.168.178.61" };

            _dispatcher = new Dispatcher.Dispatcher {
                Configuration = _cfg,
                Logger = this
            };

            var dataProvider = _dispatcher.GetDataProvider();
            dataProvider.DataChanged += OnDataChanged;
            dataProvider.CommandsReady += DataProviderOnCommandsReady;

            TrackViewer.FilePath = TrackObjectFile;

            InitializeTreeView();
        }

        private Category _itemStatus;
        private Category _itemLocomotives;
        private Category _itemS88;
        private Category _itemSwitches;
        private Category _itemRoutes;

        private void InitializeTreeView()
        {
            _itemStatus = new Category { Title = "Status" };
            _itemLocomotives = new Category { Title = "Locomotives" };
            _itemS88 = new Category { Title = "S88 Ports" };
            _itemSwitches = new Category { Title = "Switches" };
            _itemRoutes = new Category { Title = "Routes" };

            TreeViewModel.Items.Add(_itemStatus);
            TreeViewModel.Items.Add(_itemLocomotives);
            TreeViewModel.Items.Add(_itemS88);
            TreeViewModel.Items.Add(_itemSwitches);
            TreeViewModel.Items.Add(_itemRoutes);

            _dispatcher?.GetDataProvider().LoadObjects(TrackGlobalFile);
        }

        private void DataProviderOnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
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

                        _itemStatus.Title = $"{ee.Name}: {ee.Status}";

                        if (_itemStatus.Items.Count < 4)
                        {
                            _itemStatus.Items.Add(new Item { Title = $"{ee.Name}" });
                            _itemStatus.Items.Add(new Item { Title = $"Application Version: {ee.ApplicationVersion}" });
                            _itemStatus.Items.Add(new Item { Title = $"Protocol Version: {ee.ProtocolVersion}" });
                            _itemStatus.Items.Add(new Item { Title = $"Hardware Version: {ee.HardwareVersion}" });
                        }
                        else
                        {
                            _itemStatus.Items[0].Title = $"{ee.Name}";
                            _itemStatus.Items[1].Title = $"Application Version: {ee.ApplicationVersion}";
                            _itemStatus.Items[2].Title = $"Protocol Version: {ee.ProtocolVersion}";
                            _itemStatus.Items[3].Title = $"Hardware Version: {ee.HardwareVersion}";
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

        private void TreeViewModel_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = TreeViewModel.SelectedItem as Item;
            if (item != null)
            {
                if (item is Locomotive)
                {
                    _currentLocomotive = item as Locomotive;

                    if (_currentLocomotive != null)
                    {
                        var o = _currentLocomotive;
                        TxtCurrentLocomotive.Text = $"{o.ObjectId} {o.Name} ({o.Addr})";
                    }
                }
                else if (item is Switch)
                {
                    var newswitch = item as Switch;

                    if (_currentSwitch != newswitch)
                    {
                        _currentSwitch = newswitch;

                        DockSwitches.Children.Clear();

                        if (_currentSwitch != null)
                        {
                            var o = _currentSwitch;
                            TabItemSwitches.Header = "Switches: " + o.Name1;
                            DockSwitches.Children.Clear();
                            int index = 0;
                            foreach (var ad in o.Addrext)
                            {
                                var btn = new Button();
                                btn.Content = ad + " [" + index + "]";
                                btn.Click += (sender1, args) =>
                                {
                                    var oo = sender1 as Button;
                                    if (oo != null)
                                    {
                                        var txt = oo.Content.ToString();
                                        txt = txt.Substring(txt.IndexOf("[", StringComparison.OrdinalIgnoreCase));
                                        txt = txt.Trim().TrimStart('[');
                                        txt = txt.TrimEnd(']');
                                        _currentSwitch.ChangeDirection(int.Parse(txt));
                                    }
                                };
                                DockSwitches.Children.Add(btn);

                                ++index;
                            }
                        }
                    }
                }
            }
        }

        private void CmdStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (_dispatcher != null)
            {
                _dispatcher.SetRunMode(true);
            }
        }

        private void CmdStop_OnClick(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            _dispatcher?.SetRunMode(false);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Stop();
        }

        private void SliderSpeed_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_currentLocomotive == null)
                return;

            int speed = (int) SliderSpeed.Value;
            TxtCurrentSpeed.Text = $"{speed}";

            _currentLocomotive.ChangeSpeed(speed);
        }

        private void CmdF_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentLocomotive == null)
                return;

            ToggleButton btn = sender as ToggleButton;
            var txt = btn.Content.ToString().Replace("F", "").Trim();
            int ifnc;
            if (int.TryParse(txt, out ifnc))
            {
                if (btn.IsChecked.HasValue)
                {
                    if (btn.IsChecked.Value)
                    {
                        _currentLocomotive?.ToggleFunction((uint)ifnc, true);
                    }
                    else
                    {
                        _currentLocomotive?.ToggleFunction((uint)ifnc, false);
                    }
                }
            }
        }

        private void CmdFoward_OnClick(object sender, RoutedEventArgs e)
        {
            _currentLocomotive?.ChangeDirection((uint)_currentLocomotive.ObjectId, false);
        }

        private void CmdBackward_OnClick(object sender, RoutedEventArgs e)
        {
            _currentLocomotive?.ChangeDirection((uint)_currentLocomotive.ObjectId, true);
        }

        private void CmdSave_OnClick(object sender, RoutedEventArgs e)
        {
            _dispatcher?.GetDataProvider().SaveObjects(TrackGlobalFile);

            try
            {
                var trackObject = TrackViewer.Track.GetJson();
                if (trackObject != null)
                    File.WriteAllText(TrackObjectFile, trackObject.ToString(Formatting.Indented));
            }
            catch (Exception ex)
            {
                // ignore
            }
        }
    }
}
