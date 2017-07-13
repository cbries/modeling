using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ecos2Core;
using TrackInformation.Annotations;

namespace TrackInformation
{
    public class Item : IItem, INotifyPropertyChanged
    {
        public event CommandsReadyDelegator CommandsReady;

        private int _objectId;

        public int ObjectId
        {
            get => _objectId;
            set
            {
                _objectId = value;
                OnPropertyChanged();
            }
        }

        public virtual void Parse(List<CommandArgument> arguments)
        {
        }

        protected virtual void OnCommandsReady(object sender, IReadOnlyList<ICommand> commands)
        {
            if (CommandsReady != null)
                CommandsReady(sender, commands);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
