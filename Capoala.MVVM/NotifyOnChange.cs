using System;
using System.Collections.Generic;
using System.Text;

namespace Capoala.MVVM
{
    /// <summary>
    /// An attribute to specify other properties that should be notified when this property's value changes.
    /// </summary>
    public sealed class NotifyOnChange : Attribute
    {
        /// <summary>
        /// The property names to notify.
        /// </summary>
        public string[] PropertyNames { get; }

        /// <summary>
        /// Creates a new instance of <see cref="NotifyOnChange"/>.
        /// </summary>
        /// <param name="propertyNames">The property names to notify.</param>
        public NotifyOnChange(params string[] propertyNames) => PropertyNames = propertyNames;
    }
}
