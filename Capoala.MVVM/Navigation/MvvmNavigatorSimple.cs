namespace Capoala.MVVM
{
    public class SimpleMvvmNavigator<TNavigationItem>
    {
        public TNavigationItem CurrentNavigationItem { get; private set; }

        public event NavigationChangedEventHandler<TNavigationItem> NavigationDidHappen;

        public void NavigateTo(TNavigationItem navigationItem)
        {
            CurrentNavigationItem = navigationItem;
            NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(navigationItem));
        }

    }
}
