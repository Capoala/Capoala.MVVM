using System;

namespace Capoala.MVVM
{
    /// <summary>
    /// An attribute for subscribing to the change events of other properties
    /// to automate the re-query for <see cref="CommandRelay"/> or <see cref="CommandRelay{TParameter}"/> implementations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class CanExecuteDependentOn : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="CanExecuteDependentOn"/>.
        /// </summary>
        /// <param name="propertyNames">The property names to subscribe to.</param>
        public CanExecuteDependentOn(params string[] propertyNames) => PropertyNames = propertyNames;


        /// <summary>
        /// The property names to subscribe to.
        /// </summary>
        public string[] PropertyNames { get; }
    }
}
