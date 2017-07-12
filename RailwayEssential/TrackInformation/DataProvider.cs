using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private readonly List<IItem> _objects = new List<IItem>();

        public IReadOnlyList<IItem> Objects => _objects;

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
            // TODO

            return true;
        }

        private bool HandleGet(IBlock block)
        {
            switch (block.Command.ObjectId)
            {

            }

            return false;
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
                        var status = new Ecos2 { ObjectId = e.ObjectId };
                        status.Parse(e.Arguments);
                        if (!DoesObjectIdExist((uint) e.ObjectId))
                        {
                            _objects.Add(status);
                            DataChanged?.Invoke(this);
                        }
                        else
                        {
                            // update tree
                        }
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
                    var l = new S88 {ObjectId = e.ObjectId};
                    l.Parse(e.Arguments);
                    if (!DoesObjectIdExist((uint) e.ObjectId))
                    {
                        l.CommandsReady += CommandsReady;
                        _objects.Add(l);
                        DataChanged?.Invoke(this);
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
