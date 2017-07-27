namespace RailwayEssentialCore
{
    public interface IPersist
    {
        bool Save();
        bool Save(string targetFilepath);
    }
}
