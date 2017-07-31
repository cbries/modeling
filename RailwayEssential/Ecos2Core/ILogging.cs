namespace Ecos2Core
{
    public interface ILogging
    {
        void Log(string msg, params object[] args);
        void LogError(string msg, params object[] args);
        void LogNetwork(string msg, params object[] args);
    }
}
