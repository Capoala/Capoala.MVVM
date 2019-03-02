using System.Collections.Generic;
using System.Linq;

namespace Capoala.MVVM
{
    /// <summary>
    /// A base implementation for navigation.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of navigation item.</typeparam>
    public abstract class MvvmNavigatorBase<TNavigationItem>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MvvmNavigatorBase{TNavigationItem}"/>.
        /// </summary>
        protected MvvmNavigatorBase() { }


        /// <summary>
        /// A LIFO collection which stores previous navigation items.
        /// </summary>
        private Stack<TNavigationItem> BackNavigationStack { get; } = new Stack<TNavigationItem>();

        /// <summary>
        /// A LIFO collection which stores previous navigation items exclusively coming from 
        /// a "go back" operation.
        /// </summary>
        private Stack<TNavigationItem> ForwardNavigationStack { get; } = new Stack<TNavigationItem>();


        /// <summary>
        /// The current navigation item.
        /// </summary>
        public TNavigationItem CurrentNavigationItem { get; protected set; }

        /// <summary>
        /// A LIFO collection which stores previous navigation items exclusively coming from 
        /// a "go back" operation.
        /// </summary>
        public IEnumerable<TNavigationItem> BackStack => BackNavigationStack;

        /// <summary>
        /// A LIFO collection which stores previous navigation items exclusively coming from 
        /// a "go back" operation.
        /// </summary>
        public IEnumerable<TNavigationItem> ForwardStack => ForwardNavigationStack;

        /// <summary>
        /// Determines if it's possible to go back to the previous navigation item.
        /// </summary>
        public bool CanGoBack => SupportsBackNavigation && BackNavigationStack.Any();

        /// <summary>
        /// Determines if it's possible to go back to the previous navigation item
        /// after performing a "go back" operation.
        /// </summary>
        public bool CanGoForward => SupportsForwardNavigation && ForwardNavigationStack.Any();

        /// <summary>
        /// Determines whether the ability to go back to the previous
        /// navigation item is supported.
        /// </summary>
        public virtual bool SupportsBackNavigation => true;

        /// <summary>
        /// Determines whether the ability to go back to the previous
        /// navigation item after performing a "go back" operation 
        /// is supported.
        /// </summary>
        public virtual bool SupportsForwardNavigation => true;

        /// <summary>
        /// Auto clears the forward stack when performing a direct navigation
        /// via the <see cref="NavigateTo(TNavigationItem)"/> method.
        /// </summary>
        public virtual bool AutoClearForwardStackOnNavigation => true;


        /// <summary>
        /// Triggers when navigation occurs.
        /// </summary>
        public event NavigationChangedEventHandler<TNavigationItem> NavigationDidHappen;


        /// <summary>
        /// Raises the <see cref="NavigationDidHappen"/> event.
        /// </summary>
        /// <param name="navigationItem">The navigation item that triggered the event.</param>
        protected void OnNavigationDidHappen(TNavigationItem navigationItem) => NavigationDidHappen?.Invoke(this, new NavigationChangedEventArgs<TNavigationItem>(navigationItem));

        /// <summary>
        /// Navigates to the provided navigation item.
        /// </summary>
        /// <param name="navigationItem">The item to navigate to.</param>
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

        /// <summary>
        /// Clears the back stack.
        /// </summary>
        public void ClearBackStack() => BackNavigationStack.Clear();

        /// <summary>
        /// Clears the forward stack.
        /// </summary>
        public void ClearForwardStack() => ForwardNavigationStack.Clear();

        /// <summary>
        /// Navigates to the previous item.(i.e. The last item on the back stack.)
        /// </summary>
        public virtual void GoBack()
        {
            var lastItem = BackNavigationStack.Pop();

            if (SupportsForwardNavigation)
                ForwardNavigationStack.Push(CurrentNavigationItem);

            CurrentNavigationItem = lastItem;
            OnNavigationDidHappen(lastItem);
        }

        /// <summary>
        /// Navigates to the previous item from a "go back" operation.(i.e. The last item on the forward stack.)
        /// </summary>
        public virtual void GoForward()
        {
            var lastItem = ForwardNavigationStack.Pop();

            if (SupportsBackNavigation)
                BackNavigationStack.Push(CurrentNavigationItem);

            CurrentNavigationItem = lastItem;
            OnNavigationDidHappen(lastItem);
        }

        /// <summary>
        /// If <see cref="CanGoBack"/> is <see langword="true"/>, attempts to navigate to 
        /// the previous item.(i.e. The last item on the back stack.)
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if the operation completed successfully; otherwise <see langword="false"/>/
        /// </returns>
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

        /// <summary>
        /// If <see cref="CanGoForward"/> is <see langword="true"/>, attempts to navigate to 
        /// the previous item from a "go back" operation.(i.e. The last item on the forward stack.)
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if the operation completed successfully; otherwise <see langword="false"/>/
        /// </returns>
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
