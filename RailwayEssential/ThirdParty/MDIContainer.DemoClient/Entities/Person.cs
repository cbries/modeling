namespace MDIContainer.DemoClient.Entities
{
    using System;
    using Bases;

    internal class Person : ViewModelBase
    {
        public event EventHandler Changed;

        public Person(string name, DateTime birthDate, string address)
        {
            Name = name;
            BirthDate = birthDate;
            Address = address;
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                RaisePropertyChanged("BirthDate");
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                RaisePropertyChanged("Address");
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            var hander = Changed;
            hander?.Invoke(this, EventArgs.Empty);
        }
    }
}
