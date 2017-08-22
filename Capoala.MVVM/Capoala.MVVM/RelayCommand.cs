﻿using System;
using System.Windows.Input;

namespace Capoala.MVVM
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/>.
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        Func<bool> _predicate;
        Action _action;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => _predicate?.Invoke() ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter) => _action?.Invoke();

        /// <summary>
        /// Notifies the client that the ability to execute the command has changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="predicate">The method that defines whether the command can execute.</param>
        public RelayCommand(Action action, Func<bool> predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }
    }

    /// <summary>
    /// An implementation of <see cref="ICommand"/>.
    /// </summary>
    public sealed class RelayCommand<TParameter> : ICommand
    {
        Predicate<TParameter> _predicate;
        Action<TParameter> _action;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The type of object the parameter is.</param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => _predicate?.Invoke((TParameter)parameter) ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter) => _action?.Invoke((TParameter)parameter);

        /// <summary>
        /// Notifies the client that the ability to execute the command has changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand{TParameter}"/>.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="predicate">The method that defines whether the command can execute.</param>
        public RelayCommand(Action<TParameter> action, Predicate<TParameter> predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }
    }

}