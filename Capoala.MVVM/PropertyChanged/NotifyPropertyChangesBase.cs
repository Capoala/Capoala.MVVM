using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Capoala.MVVM
{
    /// <summary>
    /// A base class for implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class NotifyPropertyChangesBase : INotifyPropertyChanges
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
        /// Contains the lookup for properties of type <see cref="CommandRelay"/> with the <see cref="SubscribeToChanges"/> attribute.
        /// </summary>
        private readonly Dictionary<PropertyInfo, string[]> CommandRelaySubscribers = new Dictionary<PropertyInfo, string[]>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of <see cref="NotifyPropertyChangesBase"/>.
        /// </summary>
        protected NotifyPropertyChangesBase()
        {
            foreach (var property in GetType().GetProperties())
            {
                if (property.PropertyType == typeof(CommandRelay) || (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(CommandRelay<>)))
                {
                    var subscriptions = property.GetCustomAttributes(typeof(RequeryCanExecuteChangedOnPropertyChange), false)
                                .Cast<RequeryCanExecuteChangedOnPropertyChange>()
                                .SelectMany(attr => attr.PropertyNames)
                                .Distinct()
                                .ToArray();

                    if (subscriptions.Any())
                        CommandRelaySubscribers[property] = subscriptions;
                }
                else
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

            foreach (var subscriber in CommandRelaySubscribers)
                if (subscriber.Value.Contains(propertyName))
                    if (subscriber.Key.PropertyType.IsGenericType)
                        ((CommandRelay<dynamic>)subscriber.Key.GetValue(this)).NotifyCanExecuteDidChange();
                    else
                        ((CommandRelay)subscriber.Key.GetValue(this)).NotifyCanExecuteDidChange();
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
