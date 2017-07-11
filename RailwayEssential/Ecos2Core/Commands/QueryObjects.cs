namespace Ecos2Core.Commands
{
    public class QueryObjects : Command
    {
        public static string N = "queryObjects";
        public override CommandT Type => CommandT.QueryObjects;
        public override string Name => N;
    }
}
