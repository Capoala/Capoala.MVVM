namespace Capoala.MVVM
{
    /// <summary>
    /// A simple implementation for navigation.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of navigation item.</typeparam>
    public sealed class SimpleMvvmNavigator<TNavigationItem>
    {
        /// <summary>
        /// The current navigation item.
        /// </summary>
        public TNavigationItem CurrentNavigationItem { get; private set; }


        /// <summary>
        /// Triggers when navigation occurs.
        /// </summary>
        public event NavigationChangedEventHandler<TNavigationItem> NavigationDidHappen;


        /// <summary>
        /// Navigates to the provided navigation item.
        /// </summary>
        /// <param name="navigationItem">The item to navigate to.</param>
        public void NavigateTo(TNavigationItem navigationItem)
        {
            CurrentNavigationItem = navigationItem;
            NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(navigationItem));
        }
    }
}
