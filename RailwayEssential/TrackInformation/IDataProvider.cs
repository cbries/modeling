using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TrackInformation
{
    public delegate void DataChangedDelegator(object sender);

    public interface IDataProvider
    {
        event DataChangedDelegator DataChanged;
        event CommandsReadyDelegator CommandsReady;

        ObservableCollection<IItem> Objects { get; }

        IItem GetObjectBy(int objectid);

        bool Add(Ecos2Core.IBlock block);

        bool SaveObjects(string sessionDirectory);
        bool LoadObjects(string sessionDirectory);
    }
}
