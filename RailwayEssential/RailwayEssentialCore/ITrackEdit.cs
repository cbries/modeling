using Newtonsoft.Json.Linq;

namespace RailwayEssentialCore
{
    public interface ITrackEdit
    {
        void ChangeSymbol(int x, int y, int themeId);
        void RotateSymbol(int x, int y, string orientation);
        JArray GetJson();
    }
}
