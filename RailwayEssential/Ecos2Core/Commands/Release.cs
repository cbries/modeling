namespace Ecos2Core.Commands
{
    public class Release : Command
    {
        public static string N = "release";
        public override CommandT Type => CommandT.Release;
        public override string Name => N;
    }
}
