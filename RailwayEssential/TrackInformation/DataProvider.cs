using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Ecos2Core;
using Ecos2Core.Replies;
using TrackInformation.Annotations;

namespace TrackInformation
{    
    public class DataProvider : IDataProvider, INotifyPropertyChanged
    {
        public event DataChangedDelegator DataChanged;
        public event CommandsReadyDelegator CommandsReady;

        private readonly ObservableCollection<IItem> _objects = new ObservableCollection<IItem>();

        public ObservableCollection<IItem> Objects => _objects;

        public Ecos2 Baseobject { get; set; }

        public IItem GetObjectBy(int objectid)
        {
            if (objectid <= 0)
                return null;

            foreach (var o in _objects)
            {
                if (o == null)
                    continue;

                if (o.ObjectId == objectid)
                    return o;
            }

            return null;
        }

        private bool HandleEvent(IBlock block)
        {
            if (block == null)
                return false;

            foreach (var e in block.ListEntries)
            {
                if (e == null)
                    continue;

                IItem item = GetObjectBy(e.ObjectId);

                if (HandleEventS88(item, e))
                    continue;

                if (HandleEventSwitch(item, e))
                    continue;

                if (HandleEventLocomotive(item, e))
                    continue;
            }

            return true;
        }

        private bool HandleEventLocomotive(IItem item, ListEntry listEntry)
        {
            Locomotive e = item as Locomotive;
            if (e == null)
                return false;

            var itemLocomotive = GetObjectBy(e.ObjectId) as Locomotive;

            if (itemLocomotive != null)
            {
                itemLocomotive.Parse(listEntry.Arguments);
                itemLocomotive.UpdateTitle();
            }

            return true;
        }

        private bool HandleEventSwitch(IItem item, ListEntry listEntry)
        {
            Switch e = item as Switch;
            if (e == null)
                return false;

            var itemSwitch = GetObjectBy(e.ObjectId) as Switch;

            if (itemSwitch != null)
            {
                itemSwitch.Parse(listEntry.Arguments);
                itemSwitch.UpdateTitle();
            }

            return true;
        }

        private bool HandleEventS88(IItem item, ListEntry listEntry)
        {
            S88 e = item as S88;
            if (e == null)
                return false;

            // 1101 0000 1101 0110
            //                   0
            // 16

            string hex = listEntry.Arguments[0].Parameter[0];

            var itemS88 = GetObjectBy(e.ObjectId) as S88;
            if (itemS88 != null)
            {
                itemS88.StateOriginal = hex;
                itemS88.UpdateTitle();
            }

            return true;
        }

        public bool Add(IBlock block)
        {
            if (block == null)
                return false;

            if (block is EventBlock)
                return HandleEvent(block);

            if (block.Command == null)
                return false;

            if (block.Command.Type != CommandT.QueryObjects && block.Command.Type != CommandT.Get)
                return false;

            switch(block.Command.ObjectId)
            {
                case 1: // baseobject
                    return HandleBaseobject(block);

                case 11: // switch or route
                    return AddSwitchOrRoute(block);

                case 10: // locomotive
                    return AddLocomotive(block);

                case 26: // s88 ports
                    return AddS88(block);
            }

            return false;
        }

        public bool DoesObjectIdExist(uint objectId)
        {
            foreach (var o in _objects)
            {
                if (o?.ObjectId == objectId)
                    return true;
            }
            return false;
        }

        public void RemoveObjectWithId(uint objectId)
        {
            for (int index = 0; index < _objects.Count; ++index)
            {
                if (_objects[index].ObjectId == objectId)
                {
                    _objects.RemoveAt(index);

                    return;
                }
            }
        }

        private bool HandleBaseobject(IBlock block)
        {
            if (block == null)
                return false;

            foreach (var e in block.ListEntries)
            {
                if (e == null)
                    continue;

                switch (e.ObjectId)
                {
                    case 1:
                    {
                        if (Baseobject == null)
                            Baseobject = new Ecos2 {ObjectId = e.ObjectId};

                        Baseobject.Parse(e.Arguments);
                        if (!DoesObjectIdExist((uint) e.ObjectId))
                            _objects.Add(Baseobject);
                        DataChanged?.Invoke(this);
                    }
                        break;
                }
            }

            return true;
        }

        private bool AddSwitchOrRoute(IBlock block)
        {
            if (block == null)
                return false;

            foreach (var e in block.ListEntries)
            {
                if (e == null)
                    continue;

                string sid = $"{e.ObjectId}";

                if (sid.StartsWith("20", StringComparison.OrdinalIgnoreCase))
                {
                    var sw = new Switch { ObjectId = e.ObjectId };
                    sw.Parse(e.Arguments);
                    if (!DoesObjectIdExist((uint) e.ObjectId))
                    {
                        sw.CommandsReady += CommandsReady;
                        _objects.Add(sw);
                        DataChanged?.Invoke(this);
                        sw.EnableView();
                    }
                }
                else if (sid.StartsWith("30", StringComparison.OrdinalIgnoreCase))
                {
                    var r = new Route {ObjectId = e.ObjectId};
                    r.Parse(e.Arguments);
                    if (!DoesObjectIdExist((uint) e.ObjectId))
                    {
                        r.CommandsReady += CommandsReady;
                        _objects.Add(r);
                        DataChanged?.Invoke(this);
                    }
                }
            }

            return true;
        }

        private bool AddLocomotive(IBlock block)
        {
            if (block == null)
                return false;

            foreach (var e in block.ListEntries)
            {
                if (e == null)
                    continue;

                string sid = $"{e.ObjectId}";

                if (sid.StartsWith("10", StringComparison.OrdinalIgnoreCase))
                {
                    var l = new Locomotive {ObjectId = e.ObjectId};
                    l.Parse(e.Arguments);
                    if (!DoesObjectIdExist((uint) e.ObjectId))
                    {
                        l.CommandsReady += CommandsReady;
                        _objects.Add(l);
                        DataChanged?.Invoke(this);
                        l.EnableView();
                    }
                }
            }

            return true;
        }

        private bool AddS88(IBlock block)
        {
            if (block == null)
                return false;

            foreach (var e in block.ListEntries)
            {
                if (e == null)
                    continue;

                string sid = $"{e.ObjectId}";

                if (sid.StartsWith("10", StringComparison.OrdinalIgnoreCase))
                {
                    int n = _objects.Count(x => x is S88);

                    var s88 = new S88 {ObjectId = e.ObjectId, Index = n};
                    s88.Parse(e.Arguments);
                    if (!DoesObjectIdExist((uint) e.ObjectId))
                    {
                        s88.CommandsReady += CommandsReady;
                        _objects.Add(s88);
                        DataChanged?.Invoke(this);
                        s88.EnableView();
                    }
                }
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
