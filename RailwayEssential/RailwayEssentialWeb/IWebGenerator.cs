namespace RailwayEssentialWeb
{
    public interface IWebGenerator
    {
        bool Generate(string targetDirectory);
        string GetRandomSvg();
        string GetNextSvg();
    }
}
