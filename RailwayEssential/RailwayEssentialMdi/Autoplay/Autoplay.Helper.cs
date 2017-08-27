using RailwayEssentialMdi.Entities;

namespace RailwayEssentialMdi.Autoplay
{
    public partial class Autoplay
    {
        public void SetRoute(Analyze.Route route, bool state)
        {
            if (route == null)
                return;

            var n = route.Count;

            var trackEntity = Ctx.TrackEntity;

            for (int idx = 1; idx < n - 1; ++idx)
            {
                var r = route[idx];
                if (r == null)
                    continue;

                if (trackEntity?.Viewer != null)
                {
                    string themeIcon = null;

                    var trackInfo = trackEntity.Track.Get(r.X, r.Y);
                    if (trackInfo != null)
                    {
                        var themeInfo = Theme?.Get(trackInfo.ThemeId);
                        if (themeInfo != null)
                        {
                            if (state)
                                themeIcon = themeInfo.Off.Route;
                            else
                                themeIcon = themeInfo.Off.Default;
                        }
                    }

                    if (!string.IsNullOrEmpty(themeIcon))
                    {
                        var x = trackInfo.X;
                        var y = trackInfo.Y;
                        var themeId = trackInfo.ThemeId;
                        var orientation = trackInfo.Orientation;
                        var symbol = themeIcon;

                        trackEntity.Viewer.ExecuteJs($"changeSymbol({x}, {y}, {themeId}, \"{orientation}\", \"{symbol}\");");
                    }
                }
            }
        }

    }
}
