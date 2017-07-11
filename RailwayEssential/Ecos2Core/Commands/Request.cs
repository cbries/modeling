namespace Ecos2Core.Commands
{
    public class Request : Command
    {
        public static string N = "request";
        public override CommandT Type => CommandT.Request;
        public override string Name => N;
    }
}
