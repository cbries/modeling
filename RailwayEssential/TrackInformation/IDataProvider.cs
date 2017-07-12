using System.Collections.Generic;
using System.Windows.Input;

namespace TrackInformation
{
    public delegate void DataChangedDelegator(object sender);

    public interface IDataProvider
    {
        event DataChangedDelegator DataChanged;
        event CommandsReadyDelegator CommandsReady;

        IReadOnlyList<IItem> Objects { get; }

        IItem GetObjectBy(int objectid);

        bool Add(Ecos2Core.IBlock block);
    }
}
