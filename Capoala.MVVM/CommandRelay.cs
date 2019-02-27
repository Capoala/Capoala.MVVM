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
        /// The function which determines whether this object is allowed to execute.
        /// </summary>
        private readonly Func<bool> Predicate;

        /// <summary>
        /// The <see cref="Action"/> to execute.
        /// </summary>
        private readonly Action RelayAction;

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
        public bool CanExecute(object parameter) => Predicate?.Invoke() ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter) => RelayAction?.Invoke();

        /// <summary>
        /// Notifies the client that the ability to execute the command has changed.
        /// </summary>
        public void NotifyCanExecuteDidChange() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Creates a new instance of <see cref="CommandRelay"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="predicate">The method that defines whether the command can execute.</param>
        public CommandRelay(Action action, Func<bool> predicate = null)
        {
            RelayAction = action;
            Predicate = predicate;
        }
    }

    /// <summary>
    /// An implementation of <see cref="ICommand"/>.
    /// </summary>
    public sealed class CommandRelay<TParameter> : ICommand
    {
        /// <summary>
        /// The function which determines whether this object is allowed to execute.
        /// </summary>
        private readonly Func<bool> Predicate;

        /// <summary>
        /// The <see cref="Action{T}"/> to execute.
        /// </summary>
        private readonly Action<TParameter> RelayAction;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The type of object the parameter is.</param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => Predicate?.Invoke() ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter) => RelayAction?.Invoke((TParameter)parameter);

        /// <summary>
        /// Notifies the client that the ability to execute the command has changed.
        /// </summary>
        public void NotifyCanExecuteDidChange() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Creates a new instance of <see cref="CommandRelay{TParameter}"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="predicate">The method that defines whether the command can execute.</param>
        public CommandRelay(Action<TParameter> action, Func<bool> predicate = null)
        {
            RelayAction = action;
            Predicate = predicate;
        }
    }
}
