using System.Collections.Generic;
using System.Linq;

namespace Capoala.MVVM
{
    /// <summary>
    /// A navigation service.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of object which represents how navigation is managed.</typeparam>
    public interface INavigationService<TNavigationItem>
    {
        /// <summary>
        /// The current navigation item.
        /// </summary>
        TNavigationItem Current { get; }
        /// <summary>
        /// Navigate to the specified navigation item.
        /// </summary>
        /// <param name="navItem">The navigation item to navigate to.</param>
        void Navigate(TNavigationItem navItem);
        /// <summary>
        /// Navigates to the previous navigation item in a forward moving direction.
        /// </summary>
        void GoBack();
        /// <summary>
        /// Navigates to the previous navigation item in a backward moving direction.
        /// </summary>
        void GoForward();
        /// <summary>
        /// Determines if the ability to go back is available.
        /// </summary>
        bool CanGoBack { get; }
        /// <summary>
        /// Determines if the ability to go forward is available.
        /// </summary>
        bool CanGoForward { get; }
    }

    /// <summary>
    /// A base implementation of a navigation service.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of object which represents how navigation is managed.</typeparam>
    public abstract class NavigationServiceBase<TNavigationItem> : NotifyingObjectBase, INavigationService<TNavigationItem>
    {
        /// <summary>
        /// Creates a new <see cref="NavigationServiceBase{TNavigationItem}"/> instance.
        /// </summary>
        protected NavigationServiceBase() { }

        /// <summary>
        /// Contains previous navigation items in a forward moving direction.
        /// </summary>
        Stack<TNavigationItem> BackHistory = new Stack<TNavigationItem>();

        /// <summary>
        /// Contains previous navigation items in a backward moving direction.
        /// </summary>
        Stack<TNavigationItem> ForwardHistory = new Stack<TNavigationItem>();

        /// <summary>
        /// The current navigation item.
        /// </summary>
        public TNavigationItem Current { get => Get<TNavigationItem>(); set => Set(value); }

        /// <summary>
        /// Determines if the ability to go back is available.
        /// </summary>
        public virtual bool CanGoBack => BackHistory.Any();

        /// <summary>
        /// Determines if the ability to go forward is available.
        /// </summary>
        public virtual bool CanGoForward => ForwardHistory.Any();

        /// <summary>
        /// Navigates to the previous navigation item in a forward moving direction.
        /// </summary>
        public virtual void GoBack()
        {
            if (Current != null)
                ForwardHistory.Push(Current);
            Current = BackHistory.Pop();
        }

        /// <summary>
        /// Determines if the ability to go forward is available.
        /// </summary>
        public virtual void GoForward()
        {
            if (Current != null)
                BackHistory.Push(Current);
            Current = ForwardHistory.Pop();
        }

        /// <summary>
        /// Navigate to the specified navigation item.
        /// </summary>
        /// <param name="navItem">The navigation item to navigate to.</param>
        public virtual void Navigate(TNavigationItem navItem)
        {
            if (Current != null)
                BackHistory.Push(Current);
            Current = navItem;
        }
    }

}
