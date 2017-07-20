namespace MDIContainer.DemoClient.Entities
{
    using Bases;

    internal class Pet : ViewModelBase
    {
        public Pet(string name, string owner)
        {
            Name = name;
            Owner = owner;
        }

        public string Name { get; set; }
        public string Owner { get; set; }
    }
}
