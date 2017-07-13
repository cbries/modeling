using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Ecos2Core;

namespace RailwayEssentialUi
{
    public partial class MainWindow : Window, Ecos2Core.ILogging
    {
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

        private TrackInformation.Locomotive _currentLocomotive = null;
        private TrackInformation.Switch _currentSwitch = null;

        public MainWindow()
        {
            InitializeComponent();

            _ctx = SynchronizationContext.Current;

            _cfg = new RailwayEssentialCore.Configuration()
            {
                IpAddress = "192.168.178.61"
            };

            _dispatcher = new Dispatcher.Dispatcher
            {
                Configuration = _cfg,
                Logger = this
            };

            var dataProvider = _dispatcher.GetDataProvider();

            dataProvider.DataChanged += OnDataChanged;
            dataProvider.CommandsReady += DataProviderOnCommandsReady;
        }

        private void DataProviderOnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
            _dispatcher.ForwardCommands(commands);
        }

        private void OnDataChanged(object sender)
        {
            _ctx.Send(state =>
            {
                TreeViewModel.Items.Clear();

                var itemStatus = new ModelItem {Title = "Status"};
                var itemLocomotives = new ModelItem { Title = "Locomotives" };
                var itemS88 = new ModelItem { Title = "S88 Ports" };
                var itemSwitches = new ModelItem { Title = "Switches" };
                var itemRoutes = new ModelItem { Title = "Routes" };

                var dataProvider = _dispatcher.GetDataProvider();

                foreach (var e in dataProvider.Objects)
                {
                    if (e == null)
                        continue;

                    if (e is TrackInformation.Ecos2)
                    {
                        var ee = e as TrackInformation.Ecos2;

                        itemStatus.Title = $"{ee.Name}: {ee.Status}";
                        itemStatus.Items.Add(new ModelItem() { Object = ee, Title = $"{ee.Name}"});
                        itemStatus.Items.Add(new ModelItem() { Object = ee, Title = $"Application Version: {ee.ApplicationVersion}" });
                        itemStatus.Items.Add(new ModelItem() { Object = ee, Title = $"Protocol Version: {ee.ProtocolVersion}" });
                        itemStatus.Items.Add(new ModelItem() { Object = ee, Title = $"Hardware Version: {ee.HardwareVersion}" });
                    }
                    else if (e is TrackInformation.Locomotive)
                    {
                        var ee = e as TrackInformation.Locomotive;

                        var item = new ModelItem
                        {
                            Object = ee,
                            Title = $"{ee.ObjectId} {ee.Name} ({ee.Protocol} : {ee.Addr}"
                        };

                        itemLocomotives.Items.Add(item);
                    }
                    else if (e is TrackInformation.S88)
                    {
                        var ee = e as TrackInformation.S88;

                        var item = new ModelItem
                        {
                            Object = ee,
                            Title = $"{ee.ObjectId} {ee.Index}:{ee.Ports}"
                        };

                        itemS88.Items.Add(item);
                    }
                    else if (e is TrackInformation.Switch)
                    {
                        var ee = e as TrackInformation.Switch;

                        var ext = string.Join(", ", ee.Addrext);

                        var item = new ModelItem
                        {
                            Object = ee,
                            Title = $"{ee.ObjectId} {ee.Name1}[{ext}] {ee.SwitchState}"
                        };

                        itemSwitches.Items.Add(item);
                    }
                    else if (e is TrackInformation.Route)
                    {
                        var ee = e as TrackInformation.Route;

                        var item = new ModelItem
                        {
                            Object = ee,
                            Title = $"{ee.ObjectId} {ee.Name1}"
                        };

                        itemRoutes.Items.Add(item);
                    }
                }

                TreeViewModel.Items.Add(itemStatus);
                TreeViewModel.Items.Add(itemLocomotives);
                TreeViewModel.Items.Add(itemSwitches);
                TreeViewModel.Items.Add(itemRoutes);
                TreeViewModel.Items.Add(itemS88);
            }, null);
        }

        private void TreeViewModel_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = TreeViewModel.SelectedItem as ModelItem;
            if (item != null)
            {
                if (item.Object is TrackInformation.Locomotive)
                {
                    _currentLocomotive = item.Object as TrackInformation.Locomotive;

                    if (_currentLocomotive != null)
                    {
                        var o = _currentLocomotive;
                        TxtCurrentLocomotive.Text = $"{o.ObjectId} {o.Name} ({o.Addr})";
                    }
                } else if (item.Object is TrackInformation.Switch)
                {
                    var newswitch = item.Object as TrackInformation.Switch;

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
    }
}
