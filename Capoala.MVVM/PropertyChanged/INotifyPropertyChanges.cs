using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Capoala.MVVM
{
    /// <summary>
    /// An interface for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public interface INotifyPropertyChanges : INotifyPropertyChanged
    {
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property whose value has changed.</param>
        void Notify([CallerMemberName] string propertyName = null);

        /// <summary>
        /// Sets the referenced field to the value provided and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the given property name.
        /// </summary>
        /// <typeparam name="T">The type of value being set.</typeparam>
        /// <param name="backingField">The reference to the field being set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> implementation to use.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// Returns <see langword="true"/>if the value was set; otherwise, <see langword="false"/>.
        /// </returns>
        bool SetAndNotify<T>(ref T backingField, T value, EqualityComparer<T> equalityComparer = null, [CallerMemberName] string propertyName = null);
    }
}
