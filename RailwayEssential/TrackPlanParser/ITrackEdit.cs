using Newtonsoft.Json.Linq;

namespace TrackPlanParser
{
    public interface ITrackEdit
    {
        void ChangeSymbol(int x, int y, string symbol);
        void RotateSymbol(int x, int y, int orientation);
        JArray GetJson();
    }
}
