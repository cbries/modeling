namespace MDIContainer.DemoClient.ViewModels
{
    using System.Collections.ObjectModel;
    using Bases;
    using Commands;
    using Entities;
    using Interfaces;

    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<IContent> Items { get; }

        public ObservableCollection<Person> People { get; }

        public ObservableCollection<Pet> Pets { get; }

        public RelayCommand ShowCommand { get; }
        public RelayCommand ShowPetCommand { get; }

        private IContent _selectedWindow;
        public IContent SelectedWindow
        {
            get => _selectedWindow;
            set
            {
                _selectedWindow = value;
                RaisePropertyChanged("SelectedWindow");
            }
        }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<IContent>();
            People = new ObservableCollection<Person>();
            Pets = new ObservableCollection<Pet>();

            ShowCommand = new RelayCommand(ShowPerson, p => p != null);
            ShowPetCommand = new RelayCommand(ShowPet, p => p != null);

            People.Add(new Person("John Texas", new System.DateTime(1978, 12, 3), "NYC"));
            People.Add(new Person("Margareth Smith", new System.DateTime(1996, 4, 2), "Dallas"));
            People.Add(new Person("Jenny Happyday", new System.DateTime(1991, 5, 5), "TX"));
            People.Add(new Person("William Box", new System.DateTime(1966, 7, 3), "CA"));

            Pets.Add(new Pet("Rex", "Aunt Mary"));
            Pets.Add(new Pet("Rusty", "Oncle Bill"));
        }

        private void ShowPerson(object p)
        {
            var person = p as Person;
            if (person != null)
            {
                var item = new PersonWindow(person);
                item.Closing += (s, e) => Items.Remove(item);
                Items.Add(item);
            }
        }

        private void ShowPet(object p)
        {
            var pet = p as Pet;
            if (pet != null)
            {
                var item = new PetWindow(pet);
                Items.Add(item);
            }
        }
    }
}
