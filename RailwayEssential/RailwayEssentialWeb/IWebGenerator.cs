using TrackPlanParser;

namespace RailwayEssentialWeb
{
    public interface IWebGenerator
    {
        bool Generate(string targetFilepath);
        string GetRandomSvg();
        string GetNextSvg();
    }
}
