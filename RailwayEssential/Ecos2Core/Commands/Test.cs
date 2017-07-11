namespace Ecos2Core.Commands
{
    public class Test : Command
    {
        public static string N = "test";
        public override CommandT Type => CommandT.Test;
        public override string Name => N;
    }
}
