using System.Collections.Generic;

namespace TrackInformation
{
    public interface IDataProvider
    {
        IReadOnlyList<IItem> Objects { get; }

        IItem GetObjectBy(int objectid);

        bool Add(Ecos2Core.IBlock block);
    }
}
