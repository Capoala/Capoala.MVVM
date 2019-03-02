namespace Capoala.MVVM
{
    /// <summary>
    /// An event handler for navigation.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of navigation item.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event data.</param>
    public delegate void NavigationChangedEventHandler<TNavigationItem>(object sender, NavigationChangedEventArgs<TNavigationItem> e);
}
