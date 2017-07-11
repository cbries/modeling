using System;
using System.Collections.Generic;
using Ecos2Core;

namespace TrackInformation
{
    public class DataProvider : IDataProvider
    {
        private readonly List<IItem> _objects = new List<IItem>();

        public IReadOnlyList<IItem> Objects { get { return _objects; }}

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

        public bool Add(Ecos2Core.IBlock block)
        {
            if (block == null)
                return false;

            if (block.Command.Type != CommandT.QueryObjects)
                return false;

            switch(block.Command.ObjectId)
            {
                case 11: // switch or route
                    return AddSwitchOrRoute(block);

                case 10: // locomotive
                    return AddLocomotive(block);

                case 26: // s88 ports
                    return AddS88(block);
            }

            return false;
        }

        private bool AddSwitchOrRoute(IBlock block)
        {
            if (block == null)
                return false;

            foreach (var e in block.ListEntries)
            {
                if (e == null)
                    continue;

                string sid = string.Format("{0}", e.ObjectId);

                if (sid.StartsWith("20", StringComparison.OrdinalIgnoreCase))
                {
                    var sw = new Switch();
                    sw.Parse(e.Arguments);
                    _objects.Add(sw);
                }
                else if (sid.StartsWith("30", StringComparison.OrdinalIgnoreCase))
                {
                    var r = new Route();
                    r.Parse(e.Arguments);
                    _objects.Add(r);
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

                string sid = string.Format("{0}", e.ObjectId);

                if (sid.StartsWith("10", StringComparison.OrdinalIgnoreCase))
                {
                    var l = new Locomotive();
                    l.Parse(e.Arguments);
                    _objects.Add(l);
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

                string sid = string.Format("{0}", e.ObjectId);

                if (sid.StartsWith("10", StringComparison.OrdinalIgnoreCase))
                {
                    var l = new Locomotive();
                    l.Parse(e.Arguments);
                    _objects.Add(l);
                }
            }

            return true;
        }
    }
}
