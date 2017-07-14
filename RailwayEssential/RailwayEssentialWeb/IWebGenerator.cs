using TrackPlanParser;

namespace RailwayEssentialWeb
{
    public interface IWebGenerator
    {
        void SetTrackInfo(Track info);
        bool Generate(string targetFilepath);
        string GetRandomSvg();
        string GetNextSvg();
    }
}
