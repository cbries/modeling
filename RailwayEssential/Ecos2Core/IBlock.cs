using System.Collections.Generic;
using Ecos2Core.Replies;

namespace Ecos2Core
{
    public interface IBlock
    {
        ICommand Command { get; }
        int? ObjectId { get; }
        string NativeBlock { get; set; }
        string StartLine { get; }
        string EndLine { get; }
        ReplyResult Result { get; }
        List<ListEntry> ListEntries { get; }

        bool Parse(IReadOnlyList<string> lines);
        bool Parse(string block);
    }
}
