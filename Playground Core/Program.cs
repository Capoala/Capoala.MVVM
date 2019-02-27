using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Playground_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var yo = new Yo();

            yo.PropertyChanged += (s, e) =>
            {
                Console.WriteLine(e.PropertyName);
                System.Diagnostics.Debug.WriteLine(e.PropertyName);
            };

            yo.FirstName = "Sup?";
            yo.LastName = "Sup ?";

            Console.ReadLine();

            //System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
        }
    }





    class Yo : NotifyPropertyChangedBase
    {
        private string _firstName;
        [NotifyOnChange(nameof(FormDisplayName))]
        public string FirstName { get => _firstName; set => SetAndNotify(ref _firstName, value); }

        private string _lastName;
        public string LastName { get => _lastName; set => SetAndNotify(ref _lastName, value); }

        [SubscribeToChanges(nameof(FirstName), nameof(LastName))]
        public string FormDisplayName => $"{LastName}, {FirstName}";
    }






    /// <summary>
    /// A base class for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyPropertyChangedBaseSlim : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of <see cref="NotifyPropertyChangedBaseSlim"/>.
        /// </summary>
        protected NotifyPropertyChangedBaseSlim() { }

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

    /// <summary>
    /// A base class for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Contains the lookup of properties with the <see cref="SubscribeToChanges"/> attribute.
        /// </summary>
        private readonly Dictionary<string, string[]> Subscribers = new Dictionary<string, string[]>();

        /// <summary>
        /// Contains the lookup of properties with the <see cref="NotifyOnChange"/> attribute.
        /// </summary>
        private readonly Dictionary<string, string[]> ForcedSubscribers = new Dictionary<string, string[]>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of <see cref="NotifyPropertyChangedBase"/>.
        /// </summary>
        protected NotifyPropertyChangedBase()
        {
            foreach (var property in GetType().GetProperties())
            {
                var subscriptions = property.GetCustomAttributes(typeof(SubscribeToChanges), false)
                                            .Cast<SubscribeToChanges>()
                                            .SelectMany(attr => attr.PropertyNames)
                                            .Distinct()
                                            .ToArray();

                if (subscriptions.Any())
                    Subscribers[property.Name] = subscriptions;

                var registered = property.GetCustomAttributes(typeof(NotifyOnChange), false)
                                         .Cast<NotifyOnChange>()
                                         .SelectMany(attr => attr.PropertyNames)
                                         .Distinct()
                                         .ToArray();

                if (registered.Any())
                    ForcedSubscribers[property.Name] = registered;
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// <para>
        /// All properties subscribed to this property via the <see cref="SubscribeToChanges"/> 
        /// attribute are raised.
        /// All properties defined in the <see cref="NotifyOnChange"/> attribute associated
        /// to this property are notified.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property whose value has changed.</param>
        public virtual void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            foreach (var subscriber in Subscribers)
                if (subscriber.Value.Contains(propertyName))
                    Notify(subscriber.Key);
             
            if (ForcedSubscribers.ContainsKey(propertyName))
                foreach (var forcedSubscrober in ForcedSubscribers[propertyName])
                    Notify(forcedSubscrober);
        }

        /// <summary>
        /// Sets the referenced field to the value provided and raises the <see cref="PropertyChanged"/> event for the given property name.
        /// <para>
        /// All properties subscribed to this property via the <see cref="SubscribeToChanges"/> 
        /// attribute are raised.
        /// All properties defined in the <see cref="NotifyOnChange"/> attribute associated
        /// to this property are notified.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type of value being set.</typeparam>
        /// <param name="backingField">The reference to the field being set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> implementation to use.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// Returns <see langword="true"/>if the value was set; otherwise, <see langword="false"/>.
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

    /// <summary>
    /// A base class for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyPropertyChangedBaseAutoBackingStore : NotifyPropertyChangedBase
    {
        /// <summary>
        /// The container for storing the values for property names.
        /// </summary>
        private readonly Dictionary<string, object> BackingStore = new Dictionary<string, object>();

        /// <summary>
        /// Creates a new instance of <see cref="NotifyPropertyChangedBaseAutoBackingStore"/>.
        /// </summary>
        protected NotifyPropertyChangedBaseAutoBackingStore() { }

        /// <summary>
        /// Sets the backing value to the value provided and raises the <see cref="PropertyChanged"/> event for the given property name.
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
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// When this method returns, contains the value associated with the specified key,
        /// if the key is found; otherwise, the default value for the type of the value parameter.
        /// </returns>
        public virtual T Get<T>([CallerMemberName] string propertyName = null) => BackingStore.TryGetValue(propertyName, out var value) ? (T)value : default(T);
    }

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

