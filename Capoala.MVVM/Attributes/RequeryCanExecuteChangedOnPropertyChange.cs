using System;

namespace Capoala.MVVM
{
    /// <summary>
    /// An attribute for subscribing to the change events of other properties
    /// to automate the requery for <see cref="CommandRelay"/> or <see cref="CommandRelay{TParameter}"/> implementations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class RequeryCanExecuteChangedOnPropertyChange : Attribute
    {
        /// <summary>
        /// The property names to subscribe to.
        /// </summary>
        public string[] PropertyNames { get; }

        /// <summary>
        /// Creates a new instance of <see cref="RequeryCanExecuteChangedOnPropertyChange"/>.
        /// </summary>
        /// <param name="propertyNames">The property names to subscribe to.</param>
        public RequeryCanExecuteChangedOnPropertyChange(params string[] propertyNames) => PropertyNames = propertyNames;
    }
}
