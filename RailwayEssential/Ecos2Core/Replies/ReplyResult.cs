namespace Ecos2Core.Replies
{
    public class ReplyResult
    {
        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public void Parse(string line)
        {
            line = line.Replace("<END ", "");
            line = line.Trim().TrimEnd('\r', '\n', '>');

            int n = line.IndexOf(' ');
            if (n == -1)
            {
                ErrorCode = 0;
                ErrorMessage = "OK";
                return;
            }

            var firstPart = line.Substring(0, n).Trim();
            int v;
            if (int.TryParse(firstPart, out v))
                ErrorCode = v;
            else
                ErrorCode = 0;

            var lastPart = line.Substring(n + 1).Trim();
            lastPart = lastPart.TrimStart('(').TrimEnd(')');
            if (!string.IsNullOrEmpty(lastPart))
                ErrorMessage = lastPart;
            else
                ErrorMessage = lastPart;
        }
    }
}
