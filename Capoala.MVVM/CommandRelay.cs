using System;
using System.Windows.Input;

namespace Capoala.MVVM
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/>.
    /// </summary>
    public sealed class CommandRelay : ICommand
    {
        /// <summary>
        /// Creates a new instance of <see cref="CommandRelay"/>.
        /// </summary>
        /// <param name="executionAction">Defines the method to be called when the command is invoked.</param>
        /// <param name="canExecutePredicate">Defines the method that determines whether the command can execute in its current state.</param>
        public CommandRelay(Action executionAction, Func<bool> canExecutePredicate = null)
        {
            ExecutionAction = executionAction;
            CanExecutePredicate = canExecutePredicate;
        }


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        private readonly Func<bool> CanExecutePredicate;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        private readonly Action ExecutionAction;


        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>
        /// Returns <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanExecute(object parameter) => CanExecutePredicate?.Invoke() ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter) => ExecutionAction?.Invoke();

        /// <summary>
        /// Notifies the client that the ability to execute the command has changed.
        /// </summary>
        public void NotifyCanExecuteDidChange() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// An implementation of <see cref="ICommand"/>.
    /// </summary>
    /// <typeparam name="TExecutionParameter">The type of parameter passed to the command execution delegate.</typeparam>
    public sealed class CommandRelay<TExecutionParameter> : ICommand
    {
        /// <summary>
        /// Creates a new instance of <see cref="CommandRelay{TParameter}"/>.
        /// </summary>
        /// <param name="executionAction">Defines the method to be called when the command is invoked.</param>
        /// <param name="canExecutePredicate">Defines the method that determines whether the command can execute in its current state.</param>
        public CommandRelay(Action<TExecutionParameter> executionAction, Func<bool> canExecutePredicate = null)
        {
            ExecutionAction = executionAction;
            CanExecutePredicate = canExecutePredicate;
        }


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        private readonly Func<bool> CanExecutePredicate;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        private readonly Action<TExecutionParameter> ExecutionAction;


        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The type of object the parameter is.</param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => CanExecutePredicate?.Invoke() ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter) => ExecutionAction?.Invoke((TExecutionParameter)parameter);

        /// <summary>
        /// Notifies the client that the ability to execute the command has changed.
        /// </summary>
        public void NotifyCanExecuteDidChange() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
