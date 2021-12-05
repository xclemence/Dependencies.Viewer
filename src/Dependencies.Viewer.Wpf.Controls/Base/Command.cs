using System;
using System.Windows.Input;

namespace Dependencies.Viewer.Wpf.Controls.Base;

public class Command : Command<object>
{
    public Command(Action execute, Func<bool>? canExecute = null)
      : base(_ => execute(), canExecute == null ? null : (_) => canExecute())
    {
    }
}

public class Command<T> : ICommand
{
    private readonly Action<T> execute;
    private readonly Predicate<T?>? canExecute;

    public Command(Action<T> execute, Predicate<T?>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => canExecute?.Invoke(TransformParameter(parameter)) ?? true;

    private static T TransformParameter(object? parameter)
    {
        if (parameter is null)
            return default(T)!; // Can be null depends on Template type

        return (T)parameter;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
            execute(TransformParameter(parameter));
    }

}
