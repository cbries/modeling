namespace RailwayEssentialMdi.Entities
{
    using System;
    using System.Collections.Generic;

    public class LogMessages : Bases.ViewModelBase
    {
        public event EventHandler Changed;

        private readonly List<string> _messages = new List<string>();

        public string Message => string.Join("", _messages);

        public void Add(string text, params object[] args)
        {
            _messages.Add(string.Format(text, args));
            RaisePropertyChanged("Message");
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
