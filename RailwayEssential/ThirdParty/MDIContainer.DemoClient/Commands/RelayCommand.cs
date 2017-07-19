﻿namespace MDIContainer.DemoClient.Commands
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public class RelayCommand : ICommand
    {
        private Action<object> ExecuteAction { get; }
        private Predicate<object> CanExecutePredicate { get; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> executeAction)
           : this(executeAction, p => true)
        {
        }

        public RelayCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            ExecuteAction = executeAction;
            CanExecutePredicate = canExecutePredicate;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return CanExecutePredicate == null || CanExecutePredicate(parameter);
        }

        [DebuggerStepThrough]
        public void Execute(object parameter)
        {
            ExecuteAction?.Invoke(parameter);
        }

        [DebuggerStepThrough]
        public void InvalidateRequerySuggested()
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                dispatcher.BeginInvoke(new Action(CommandManager.InvalidateRequerySuggested));
            }
        }
    }

}
