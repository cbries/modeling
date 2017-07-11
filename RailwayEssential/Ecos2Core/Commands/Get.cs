namespace Ecos2Core.Commands
{
    public class Get : Command
    {
        public static string N = "get";
        public override CommandT Type => CommandT.Get;
        public override string Name => N;
    }
}
