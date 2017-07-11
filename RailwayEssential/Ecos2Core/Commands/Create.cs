namespace Ecos2Core.Commands
{
    public class Create : Command
    {
        public static string N = "create";
        public override CommandT Type => CommandT.Create;
        public override string Name => N;
    }
}
