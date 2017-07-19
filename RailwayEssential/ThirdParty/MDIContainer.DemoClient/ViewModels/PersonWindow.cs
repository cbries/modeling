using System;
using System.Windows;

using MDIContainer.DemoClient.Bases;
using MDIContainer.DemoClient.Commands;
using MDIContainer.DemoClient.Entities;
using MDIContainer.DemoClient.Interfaces;

namespace MDIContainer.DemoClient.ViewModels
{
   public class PersonWindow : ViewModelBase, IContent
   {
      public string Title => Person.Name;

       public event EventHandler Closing;

      public RelayCommand CloseCommand { get; }      

      public Person Person { get; }

      private bool IsDirty { get; set; }

      public PersonWindow(Person person)
      {
         Person = person;
         Person.Changed += (s, e) => IsDirty = true;

         CloseCommand = new RelayCommand(CloseWindow);
      }

      private void CloseWindow(object p)
      {
         if (CanClose)
         {
            var hander = Closing;
             hander?.Invoke(this, EventArgs.Empty);
         }
      }      

      public bool CanClose => IsDirty == false || MessageBox.Show("Changes were made. Do you want to close this window?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
   }
}
