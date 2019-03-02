using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Capoala.MVVM
{
    /// <summary>
    /// A base class for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyPropertyChangesBaseSlim : INotifyPropertyChanges
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of <see cref="NotifyPropertyChangesBaseSlim"/>.
        /// </summary>
        protected NotifyPropertyChangesBaseSlim() { }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property whose value has changed.</param>
        public virtual void Notify([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Sets the referenced field to the value provided and raises the <see cref="PropertyChanged"/> event for the given property name.
        /// </summary>
        /// <typeparam name="T">The type of value being set.</typeparam>
        /// <param name="backingField">The reference to the field being set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> implementation to use.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// Returns <see langword="true"/>if the value was set; otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool SetAndNotify<T>(ref T backingField, T value, EqualityComparer<T> equalityComparer = null, [CallerMemberName] string propertyName = null)
        {
            if ((equalityComparer ?? EqualityComparer<T>.Default).Equals(value, backingField))
            {
                return false;
            }
            else
            {
                backingField = value;
                Notify(propertyName);
                return true;
            }
        }
    }
}
