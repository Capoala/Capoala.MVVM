using System;
using System.Collections.Generic;
using System.Text;

namespace Capoala.MVVM
{
    /// <summary>
    /// An attribute for subscribing to the change events of other properties.
    /// </summary>
    public sealed class SubscribeToChanges : Attribute
    {
        /// <summary>
        /// The property names to subscribe to.
        /// </summary>
        public string[] PropertyNames { get; }

        /// <summary>
        /// Creates a new instance of <see cref="SubscribeToChanges"/>.
        /// </summary>
        /// <param name="propertyNames">The property names to subscribe to.</param>
        public SubscribeToChanges(params string[] propertyNames) => PropertyNames = propertyNames;
    }
}
