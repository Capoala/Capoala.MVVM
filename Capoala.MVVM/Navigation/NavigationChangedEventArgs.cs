using System;

namespace Capoala.MVVM
{
    /// <summary>
    /// An event argument representing a navigation item.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of navigation item.</typeparam>
    public class NavigationChangedEventArgs<TNavigationItem> : EventArgs
    {
        /// <summary>
        /// The navigation item that triggered the event.
        /// </summary>
        public TNavigationItem NavigationItem { get; }

        /// <summary>
        /// Creates a new instance of <see cref="NavigationChangedEventArgs{TNavigationItem}"/>.
        /// </summary>
        /// <param name="navigationItem">The navigation item that triggered the event.</param>
        public NavigationChangedEventArgs(TNavigationItem navigationItem) => NavigationItem = navigationItem;
    }
}
