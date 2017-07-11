namespace Ecos2Core.Commands
{
    public class Set : Command
    {
        public static string N = "set";
        public override CommandT Type => CommandT.Set;
        public override string Name => N;
    }
}
