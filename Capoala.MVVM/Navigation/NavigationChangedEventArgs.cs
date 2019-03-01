using System;

namespace Capoala.MVVM
{
    public class NavigationChangedEventArgs<TNavigationItem> : EventArgs
    {
        public TNavigationItem NavigationItem { get; }

        public NavigationChangedEventArgs(TNavigationItem navigationItem) => NavigationItem = navigationItem;
    }
}
