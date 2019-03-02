using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Capoala.MVVM
{
    /// <summary>
    /// A base class for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyPropertyChangesBaseAutoBackingStore : NotifyPropertyChangesBase
    {
        /// <summary>
        /// The container for storing the values for property names.
        /// </summary>
        private readonly Dictionary<string, object> BackingStore = new Dictionary<string, object>();

        /// <summary>
        /// Creates a new instance of <see cref="NotifyPropertyChangesBaseAutoBackingStore"/>.
        /// </summary>
        protected NotifyPropertyChangesBaseAutoBackingStore() { }

        /// <summary>
        /// Sets the backing value to the value provided and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the given property name.
        /// <para>
        /// All properties subscribed to this property via the <see cref="SubscribeToChanges"/> 
        /// attribute are raised.
        /// All properties defined in the <see cref="NotifyOnChange"/> attribute associated
        /// to this property are notified.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type of value being set.</typeparam>
        /// <param name="value">The value to set.</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> implementation to use.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// Returns <see langword="true"/>if the value was set; otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Set<T>(T value, EqualityComparer<T> equalityComparer = null, [CallerMemberName] string propertyName = null)
        {
            if (!equalityComparer?.Equals(value, Get<T>(propertyName)) ?? !EqualityComparer<T>.Default.Equals(value, Get<T>(propertyName)))
            {
                BackingStore[propertyName] = value;
                Notify(propertyName);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the underlying value for the given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// When this method returns, contains the value associated with the specified key,
        /// if the key is found; otherwise, the default value for the type of the value parameter.
        /// </returns>
        public virtual T Get<T>([CallerMemberName] string propertyName = null) => BackingStore.TryGetValue(propertyName, out var value) ? (T)value : default;
    }
}
