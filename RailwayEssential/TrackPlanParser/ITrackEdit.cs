using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public interface ITrackEdit
    {
        void ChangeSymbol(int x, int y, int themeId);
        void RotateSymbol(int x, int y, string orientation);
        JArray GetJson();
    }
}
