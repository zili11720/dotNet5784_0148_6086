﻿using System.Windows.Input;
#nullable disable

namespace PL.Gantt;

public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> execute;
    private readonly Predicate<T> canExecute;

    public DelegateCommand(Action<T> execute)
        : this(execute, null!) { }

    public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
    {
        if (execute == null)
            throw new ArgumentNullException("Execute can not be null");

        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => canExecute == null ? true : canExecute((T)parameter);

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object theParameter) => execute((T)theParameter);
}
