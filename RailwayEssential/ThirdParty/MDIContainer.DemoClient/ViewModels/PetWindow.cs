namespace MDIContainer.DemoClient.ViewModels
{
   using Bases;
   using Entities;
   using Interfaces;

   public class PetWindow : ViewModelBase, IContent
   {
      public string Title => string.Format("{0} - {1}", Pet.Name, Pet.Owner);

       public PetWindow(Pet pet)
      {
         Pet = pet;
      }

      public Pet Pet { get; }

      public bool CanClose => true;
   }
}
