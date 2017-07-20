using System.Windows;

namespace MDIContainer.Control.Events
{
   public sealed class WindowStateChangedEventArgs : RoutedEventArgs
   {
      public WindowState OldValue { get; }
      public WindowState NewValue { get; }

      public WindowStateChangedEventArgs(RoutedEvent routedEvent, WindowState oldValue, WindowState newValue)
         : base(routedEvent)
      {
         NewValue = newValue;
         OldValue = oldValue;                  
      }
   }
}
