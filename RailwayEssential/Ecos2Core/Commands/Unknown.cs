namespace Ecos2Core.Commands
{
    public class Unknown : Command
    {
        public static string N = "Unknown";
        public override CommandT Type => CommandT.Unknown;
        public override string Name => N;
    }
}
