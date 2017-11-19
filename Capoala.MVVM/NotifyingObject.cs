using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Capoala.MVVM
{
    /// <summary>
    /// Allows the ability to notify clients that a property has changed.
    /// </summary>
    public abstract class NotifyingObjectBaseSlim : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified property name.
        /// </summary>
        /// <param name="caller">The property name.</param>
        public virtual void Notify([CallerMemberName] string caller = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));

        /// <summary>
        /// Sets the referenced property to the given value and then raised the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="property">The property reference to set the value of.</param>
        /// <param name="value">The value to set the proeprty to.</param>
        /// <param name="caller">The property name.</param>
        public virtual void SetAndNotify<TProperty>(ref TProperty property, object value, [CallerMemberName] string caller = null)
        {
            property = (TProperty)value;
            Notify(caller);
        }
    }

    /// <summary>
    /// Allows the ability to notify clients that a property has changed.
    /// </summary>
    public abstract class NotifyingObjectBase : INotifyPropertyChanged
    {
        /// <summary>
        /// A collection associating property names with a collection of property names to notify.
        /// </summary>
        KeyValuePair<string, string[]>[] NotifiedOnChangeAssociations;

        /// <summary>
        /// A collection associating property names with a collection of property names to notify.
        /// </summary>
        KeyValuePair<string, string[]>[] NotifyOnChangeAssociations;

        /// <summary>
        /// A query for property names to notify when this property is raised.
        /// </summary>
        /// <param name="caller">The property name being raised.</param>
        /// <returns></returns>
        IEnumerable<string> NotifiedOnChangePropertyNamesQuery(string caller)
            => NotifiedOnChangeAssociations.Where(kvp => kvp.Value.Contains(caller)).Select(kvp => kvp.Key);

        /// <summary>
        /// A query for property names to notify when this property is raised.
        /// </summary>
        /// <param name="caller">The property name being raised.</param>
        /// <returns></returns>
        IEnumerable<string> NotifyOnChangePropertyNamesQuery(string caller)
            => NotifyOnChangeAssociations.Where(kvp => kvp.Key == caller).SelectMany(kvp => kvp.Value);

        Dictionary<string, object> _backingStore = new Dictionary<string, object>();
        /// <summary>
        /// A collection storing property names and their associated values.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> BackingStore => _backingStore;

        /// <summary>
        /// Creates a new <see cref="NotifyingObjectBase"/> instance.
        /// </summary>
        protected NotifyingObjectBase()
        {
            var notified = GetType().GetProperties()
                                    .Where(p => p.GetCustomAttributes(typeof(NotifiedOnChange), false).Any());

            var notify = GetType().GetProperties()
                                  .Where(p => p.GetCustomAttributes(typeof(NotifyOnChange), false).Any());

            NotifiedOnChangeAssociations
                = notified.Select(propertyInfo =>
                    new KeyValuePair<string, string[]>(
                        propertyInfo.Name,
                        propertyInfo.GetCustomAttributes(typeof(NotifiedOnChange), false)
                                    .SelectMany(attr => ((NotifiedOnChange)attr).PropertyNames)
                                    .ToArray()))
                          .ToArray();

            NotifyOnChangeAssociations
                = notify.Select(propertyInfo =>
                    new KeyValuePair<string, string[]>(
                        propertyInfo.Name,
                        propertyInfo.GetCustomAttributes(typeof(NotifyOnChange), false)
                                    .SelectMany(attr => ((NotifyOnChange)attr).PropertyNames)
                                    .ToArray()))
                          .ToArray();
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified property name.
        /// </summary>
        /// <param name="caller">The property name.</param>
        public virtual void Notify([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));

            foreach (var propertyToNotify in NotifyOnChangePropertyNamesQuery(caller).Concat(NotifiedOnChangePropertyNamesQuery(caller)).Distinct())
                Notify(propertyToNotify);
        }

        /// <summary>
        /// Sets the value of the specified property and raises a property changed event.
        /// </summary>
        /// <typeparam name="TValue">The type of object to store.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">The property name.</param>
        public virtual void Set<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<TValue>.Default.Equals(value, Get<TValue>(propertyName)))
            {
                _backingStore[propertyName] = value;
                Notify(propertyName);
            }
        }

        /// <summary>
        /// Retrieves the value associated to the specified property name.
        /// </summary>
        /// <typeparam name="TValue">The type of object being retrieved.</typeparam>
        /// <param name="propertyName">The property name to retrieve the value of.</param>
        /// <returns></returns>
        public virtual TValue Get<TValue>([CallerMemberName] string propertyName = null)
        {
            if (_backingStore.TryGetValue(propertyName, out var value))
                return (TValue)value;
            else
                return default(TValue);
        }

        /// <summary>
        /// Subscribes to the <see cref="PropertyChanged"/> event of one or more other properties.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public sealed class NotifiedOnChange : Attribute
        {
            /// <summary>
            /// The collection of property names that should trigger a raise of this property.
            /// </summary>
            public string[] PropertyNames { get; private set; }

            /// <summary>
            /// Creates a new <see cref="NotifiedOnChange"/> instance.
            /// </summary>
            /// <param name="propertyNames">The collection of property names that should trigger a raise of this property.</param>
            public NotifiedOnChange(params string[] propertyNames) => PropertyNames = propertyNames;
        }

        /// <summary>
        /// Notifies the specified properties when this property's value changes.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public sealed class NotifyOnChange : Attribute
        {
            /// <summary>
            /// The collection of property names that should be raised when this property's value is changed.
            /// </summary>
            public string[] PropertyNames { get; private set; }

            /// <summary>
            /// Creates a new <see cref="NotifyOnChange"/> instance.
            /// </summary>
            /// <param name="propertyNames">The collection of property names that should be raised when this property's value is changed.</param>
            public NotifyOnChange(params string[] propertyNames) => PropertyNames = propertyNames;
        }
    }
}
