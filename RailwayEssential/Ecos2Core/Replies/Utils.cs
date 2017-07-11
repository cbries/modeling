using System;
using System.Collections.Generic;

namespace Ecos2Core.Replies
{
    public static class Utils
    {
        public enum BlockT
        {
            Reply, Event
        }

        public static bool HasAnyBlock(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return false;

            var m = msg.Trim();

            bool r0 = m.StartsWith("<REPLY ", StringComparison.OrdinalIgnoreCase)
                   && m.IndexOf("<END ", StringComparison.OrdinalIgnoreCase) != -1;

            if (r0)
                return true;

            return m.StartsWith("<EVENT ", StringComparison.OrdinalIgnoreCase)
                && m.IndexOf("<END ", StringComparison.OrdinalIgnoreCase) != -1;
        }

        public static bool HasAnyBlock(IReadOnlyList<string> lines)
        {
            if (lines == null)
                return false;
            if (lines.Count < 2)
                return false;

            var firstLine = lines[0];
            var lastLine = "";

            foreach (var line in lines)
            {
                var l = line.Trim();

                if (string.IsNullOrEmpty(l))
                    continue;

                lastLine = l;
            }

            bool r = firstLine.Trim().StartsWith("<REPLY ", StringComparison.OrdinalIgnoreCase)
                  && lastLine.Trim().StartsWith("<END ", StringComparison.OrdinalIgnoreCase);

            if (r)
                return true;

            return firstLine.Trim().StartsWith("<EVENT ", StringComparison.OrdinalIgnoreCase)
                && lastLine.Trim().StartsWith("<END ", StringComparison.OrdinalIgnoreCase);
        }

        public static IReadOnlyList<IBlock> GetBlocks(IReadOnlyList<string> lines)
        {
            List<IBlock> blocks = new List<IBlock>();
            List<string> blockLines = new List<string>();
            for (int i = 0; i < lines.Count; ++i)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;

                var line = lines[i];

                if (string.IsNullOrEmpty(line))
                    continue;

                line = line.TrimStart('\r', '\n');

                if (line.StartsWith("<END ", StringComparison.OrdinalIgnoreCase))
                {
                    blockLines.Add(line + "\r\n");

                    var firstLine = blockLines[0];
                    IBlock instance = null;

                    if (firstLine.StartsWith("<EVENT ", StringComparison.OrdinalIgnoreCase))
                        instance = new EventBlock();
                    else if(firstLine.StartsWith("<REPLY ", StringComparison.OrdinalIgnoreCase))
                        instance = new ReplyBlock();

                    if (instance != null)
                    {
                        if (!instance.Parse(blockLines))
                            return null;
                        blocks.Add(instance);
                        blockLines.Clear();
                    }
                }
                else
                {
                    blockLines.Add(line + "\r\n");
                }
            }

            return blocks;
        }
    }
}
