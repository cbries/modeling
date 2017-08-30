using System;
using System.IO;
using TrackPlanParser;
using TrackWeaver;

namespace RailwayEssentialMdi
{
    internal class Helper
    {
        public static int GetOrientation(TrackInfo info)
        {
            if (info == null)
                return 0;
            if (string.IsNullOrEmpty(info.Orientation))
                return 0;
            if (info.Orientation.Equals("rot0", StringComparison.OrdinalIgnoreCase))
                return 0;
            if (info.Orientation.Equals("rot90", StringComparison.OrdinalIgnoreCase))
                return 1;
            if (info.Orientation.Equals("rot180", StringComparison.OrdinalIgnoreCase))
                return 2;
            if (info.Orientation.Equals("rot-90", StringComparison.OrdinalIgnoreCase))
                return 3;
            return 0;
        }

        public static TrackInformationCore.IItem GetObject(Dispatcher.Dispatcher dispatcher, Track track, int x, int y
        ) {
            var trackInfo = track.Get(x, y);

            if (trackInfo == null)
                return null;

            var weaver = dispatcher.Weaver;
            if (weaver != null)
            {
                var ws = weaver.WovenSeam;
                if (ws != null)
                {
                    foreach (var seam in ws)
                    {
                        if (seam == null)
                            continue;

                        if (seam.TrackObjects.ContainsKey(trackInfo))
                            return seam.ObjectItem;
                    }
                }
            }

            return null;
        }

        /// <summary> Call of GetWeaveItem(..) can be speed up by NOT reading the weave file on any call. </summary>
        /// <param name="dispatcher"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static TrackWeaveItem GetWeaveItem(Dispatcher.Dispatcher dispatcher, int x, int y)
        {
            var m = dispatcher.Model as ViewModels.RailwayEssentialModel;
            if (m == null)
                return null;

            var prj = m.Project;

            var weaveFilepath = Path.Combine(prj.Dirpath, prj.Track.Weave);
            TrackWeaveItems weaverItems = new TrackWeaveItems();
            if (!weaverItems.Load(weaveFilepath))
                return null;

            foreach (var e in weaverItems.Items)
            {
                if (e?.VisuX == x && e.VisuY == y)
                    return e;
            }

            return null;
        }
    }
}
