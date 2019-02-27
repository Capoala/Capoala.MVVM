using System.Collections.Generic;
using System.Linq;

namespace Capoala.MVVM
{
    public abstract class MvvmNavigatorBase<TNavigationItem> : IMvvmNavigator<TNavigationItem>
    {
        protected MvvmNavigatorBase() { }

        private Stack<TNavigationItem> BackNavigationStack { get; } = new Stack<TNavigationItem>();

        private Stack<TNavigationItem> ForwardNavigationStack { get; } = new Stack<TNavigationItem>();

        public TNavigationItem CurrentNavigationItem { get; protected set; }

        public bool CanGoBack => BackNavigationStack.Any();

        public bool CanGoForward => ForwardNavigationStack.Any();

        public event NavigationChangedEventHandler<TNavigationItem> NavigationDidHappen;

        public void GoBack()
        {
            var lastItem = BackNavigationStack.Pop();
            ForwardNavigationStack.Push(lastItem);
            CurrentNavigationItem = lastItem;
            NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(lastItem));
        }

        public void GoForward()
        {
            var lastItem = ForwardNavigationStack.Pop();
            BackNavigationStack.Push(lastItem);
            CurrentNavigationItem = lastItem;
            NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(lastItem));
        }

        public void NavigateTo(TNavigationItem navigationItem)
        {
            BackNavigationStack.Push(navigationItem);
            CurrentNavigationItem = navigationItem;
            NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(navigationItem));
        }

        public bool TryGoBack()
        {
            if (CanGoBack)
            {
                GoBack();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGoForward()
        {
            if (CanGoForward)
            {
                GoForward();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
