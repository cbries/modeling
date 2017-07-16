using System;
using System.Collections.Generic;
using TrackInformationCore;
using TrackPlanParser;

namespace TrackWeaver
{
    public class TrackWeaverItem
    {
        public IItem ObjectItem { get; set; }
        public Dictionary<TrackInfo, Func<bool>> TrackObjects { get; set; }
    }

    public class TrackWeaver
    {
        private readonly List<TrackWeaverItem> _associations = new List<TrackWeaverItem>();

        public List<TrackWeaverItem> WovenSeam => _associations;

        public TrackWeaver()
        { }

        private TrackWeaverItem GetItem(IItem item)
        {
            foreach (var e in _associations)
            {
                if (e == null)
                    continue;

                if (e.ObjectItem == item)
                    return e;
            }

            return null;
        }

        public void Link(IItem item, TrackInfo trackObject, Func<bool> fncCheckState)
        {
            if (item == null || trackObject == null)
                return;

            var e = GetItem(item);
            if (e != null)
            {
                if (e.TrackObjects == null)
                    e.TrackObjects = new Dictionary<TrackInfo, Func<bool>>();

                if (e.TrackObjects.ContainsKey(trackObject))
                    e.TrackObjects[trackObject] = fncCheckState;
                else
                    e.TrackObjects.Add(trackObject, fncCheckState);
            }
            else
            {
                e = new TrackWeaverItem
                {
                    ObjectItem = item,
                    TrackObjects = new Dictionary<TrackInfo, Func<bool>>
                    {
                        {trackObject, fncCheckState}
                    }
                };

                _associations.Add(e);
            }

        }

        public void UnLink(TrackInfo trackObject)
        {
            if (trackObject == null)
                return;

            foreach (var e in _associations)
            {
                if (e == null)
                    continue;

                if (e.TrackObjects.ContainsKey(trackObject))
                    e.TrackObjects.Remove(trackObject);
            }
        }

        public void UnLink(IItem item)
        {
            if (item == null)
                return;

            var e = GetItem(item);
            if (e != null)
                _associations.Remove(e);
        }
    }
}
