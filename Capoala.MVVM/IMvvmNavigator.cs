namespace Capoala.MVVM
{
    public interface IMvvmNavigator<TNavigationItem>
    {
        event NavigationChangedEventHandler<TNavigationItem> NavigationDidHappen;
        TNavigationItem CurrentNavigationItem { get; }
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        bool TryGoBack();
        bool TryGoForward();
        void GoBack();
        void GoForward();
        void NavigateTo(TNavigationItem navigationItem);
    }
}
