using System.Collections.Generic;
using System.Linq;

namespace Capoala.MVVM
{
    public abstract class MvvmNavigatorBase<TNavigationItem>
    {
        protected MvvmNavigatorBase() { }


        private Stack<TNavigationItem> BackNavigationStack { get; } = new Stack<TNavigationItem>();

        private Stack<TNavigationItem> ForwardNavigationStack { get; } = new Stack<TNavigationItem>();


        public TNavigationItem CurrentNavigationItem { get; protected set; }

        public bool CanGoBack => BackNavigationStack.Any();

        public bool CanGoForward => ForwardNavigationStack.Any();

        public IEnumerable<TNavigationItem> BackStack => BackNavigationStack;

        public IEnumerable<TNavigationItem> ForwardStack => ForwardNavigationStack;

        public virtual bool SupportsBackNavigation => true;

        public virtual bool SupportsForwardNavigation => true;

        public virtual bool AutoClearForwardStackOnNavigation => false;


        public event NavigationChangedEventHandler<TNavigationItem> NavigationDidHappen;


        protected void OnNavigationDidHappen(TNavigationItem navigationItem) => NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(navigationItem));

        public void ClearBackStack() => BackNavigationStack.Clear();

        public void ClearForwardStack() => ForwardNavigationStack.Clear();

        public virtual void GoBack()
        {
            var lastItem = BackNavigationStack.Pop();

            if (SupportsForwardNavigation)
                ForwardNavigationStack.Push(CurrentNavigationItem);

            CurrentNavigationItem = lastItem;
            OnNavigationDidHappen(lastItem);
        }

        public virtual void GoForward()
        {
            var lastItem = ForwardNavigationStack.Pop();

            if (SupportsBackNavigation)
                BackNavigationStack.Push(CurrentNavigationItem);

            CurrentNavigationItem = lastItem;
            OnNavigationDidHappen(lastItem);
        }

        public virtual void NavigateTo(TNavigationItem navigationItem)
        {
            if (SupportsBackNavigation)
                if (CurrentNavigationItem != null)
                    BackNavigationStack.Push(CurrentNavigationItem);

            if (AutoClearForwardStackOnNavigation)
                ClearForwardStack();

            CurrentNavigationItem = navigationItem;
            OnNavigationDidHappen(navigationItem);
        }

        public virtual bool TryGoBack()
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

        public virtual bool TryGoForward()
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
