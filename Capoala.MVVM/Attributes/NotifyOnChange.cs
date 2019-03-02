using System;

namespace Capoala.MVVM
{
    /// <summary>
    /// An attribute to specify other properties that should be notified when this property's value changes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class NotifyOnChange : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="NotifyOnChange"/>.
        /// </summary>
        /// <param name="propertyNames">The property names to notify.</param>
        public NotifyOnChange(params string[] propertyNames) => PropertyNames = propertyNames;


        /// <summary>
        /// The property names to notify.
        /// </summary>
        public string[] PropertyNames { get; }
    }
}
