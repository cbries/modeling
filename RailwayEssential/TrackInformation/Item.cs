﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Ecos2Core;
using Newtonsoft.Json.Linq;
using RailwayEssentialCore;
using TrackInformation.Annotations;
using TrackInformationCore;

namespace TrackInformation
{
    public class Item : IItem, INotifyPropertyChanged
    {
        public event CommandsReadyDelegator CommandsReady;

        public IRailwayEssentialModel Ctx { get; set; }

        public virtual string IconName { get; set; }

        public ObservableCollection<Item> Items { get; set; }

        public virtual int TypeId()
        {
            return -1;
        }

        public bool HasView { get; private set; }

        private bool _isRouted;

        public bool IsRouted
        {
            get => _isRouted;
            set
            {
                _isRouted = value;
                OnPropertyChanged();
            }
        }

        private string _title, _subTitle;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("SubTitle");
                OnPropertyChanged("SubTitleHeight");
            }
        }

        public string SubTitle
        {
            get => _subTitle;
            set
            {
                _subTitle = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("SubTitle");
                OnPropertyChanged("SubTitleHeight");
            }
        }

        public virtual int SubTitleHeight => 0;

        private int _objectId;

        public int ObjectId
        {
            get => _objectId;
            set
            {
                _objectId = value;
                OnPropertyChanged();
            }
        }

        public Item()
        {
            Items = new ObservableCollection<Item>();
            ObjectId = -1;
        }

        #region Icon stuff

        private BitmapSource _iconSource = null;

        public BitmapSource IconSource
        {
            get
            {
                if (_iconSource != null)
                    return _iconSource;

                UpdateIconPath();

                return _iconSource;
            }
        }

        protected virtual void UpdateIconPath()
        {
            string BasePackUrlsPath = @"pack://application:,,,/RailwayEssentialMdi;component/Resources/";

            var item = this as IItem;

            if (item == null)
            {
                _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "unknown.png"));
            }
            else
            {
                if (!string.IsNullOrEmpty(IconName))
                    _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + IconName));
                else
                {
                    if (item is Locomotive)
                        _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "train.png"));
                    else if (item is Switch)
                        _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "switch.png"));
                    else if (item is Route)
                        _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "route.png"));
                    else if (item is S88)
                        _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "s88.png"));
                    else
                        _iconSource = new BitmapImage(new Uri(BasePackUrlsPath + "unknown.png"));
                }
            }
        }

        #endregion

        public virtual void UpdateTitle()
        {
            OnPropertyChanged("Title");
        }

        public virtual void UpdateSubTitle()
        {
            OnPropertyChanged("SubTitle");
        }

        public void EnableView()
        {
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"request({ObjectId}, view)")
            };

            OnCommandsReady(this, ctrlCmds);

            HasView = true;
        }

        public void DisableView()
        {
            List<ICommand> ctrlCmds = new List<ICommand>
            {
                CommandFactory.Create($"release({ObjectId}, view)")
            };

            OnCommandsReady(this, ctrlCmds);

            HasView = false;
        }

        public virtual void Parse(List<CommandArgument> arguments)
        {
        }

        public virtual JObject ToJson()
        {
            return null;
        }

        public virtual void ParseJson(JObject obj)
        {

        }

        protected virtual void OnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
            if (CommandsReady != null)
                CommandsReady(sender, commands);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChange(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            OnPropertyChanged(name);
        }
    }
}
