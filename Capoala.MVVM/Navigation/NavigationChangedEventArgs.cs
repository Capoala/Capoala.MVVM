using System;

namespace Capoala.MVVM
{
    /// <summary>
    /// An event argument representing a navigation item.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of navigation item.</typeparam>
    public sealed class NavigationChangedEventArgs<TNavigationItem> : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="NavigationChangedEventArgs{TNavigationItem}"/>.
        /// </summary>
        /// <param name="navigationItem">The navigation item that triggered the event.</param>
        public NavigationChangedEventArgs(TNavigationItem navigationItem) => NavigationItem = navigationItem;


        /// <summary>
        /// The navigation item that triggered the event.
        /// </summary>
        public TNavigationItem NavigationItem { get; }
    }
}
