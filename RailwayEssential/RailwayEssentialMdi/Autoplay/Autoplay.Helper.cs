using System;

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

                        var isSwitch = RailwayEssentialCore.Globals.SwitchIds.Contains(themeId);

                        if (r.HasTurn && isSwitch)
                        {
                            var parts = symbol.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
                            if(parts.Length == 2)
                                symbol = parts[0] + "-t-" + parts[1];
                            else if (parts.Length == 1)
                                symbol = parts[0] + "-t";
                        }

                        trackEntity.Viewer.ExecuteJs($"changeSymbol({x}, {y}, {themeId}, \"{orientation}\", \"{symbol}\");");
                    }
                }
            }
        }

    }
}
