namespace Capoala.MVVM
{
    /// <summary>
    /// A default implementation of <see cref="MvvmNavigatorBase{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">The type of navigation item.</typeparam>
    public sealed class MvvmNavigator<TNavigationItem> : MvvmNavigatorBase<TNavigationItem>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MvvmNavigator{TNavigationItem}"/>.
        /// </summary>
        /// <param name="supportsBackNavigation">
        /// Determines whether the ability to go back to the previous
        /// navigation item is supported.
        /// </param>
        /// <param name="supportsForwardNavigation">
        /// Determines whether the ability to go back to the previous
        /// navigation item after performing a "go back" operation 
        /// is supported.
        /// </param>
        /// <param name="autoClearForwardStackOnNavigation">
        /// Auto clears the forward stack when performing a direct navigation.
        /// </param>
        public MvvmNavigator(bool supportsBackNavigation = true, bool supportsForwardNavigation = true, bool autoClearForwardStackOnNavigation = true)
        {
            SupportsBackNavigation = supportsBackNavigation;
            SupportsForwardNavigation = supportsForwardNavigation;
            AutoClearForwardStackOnNavigation = autoClearForwardStackOnNavigation;
        }


        /// <summary>
        /// Determines whether the ability to go back to the previous
        /// navigation item is supported.
        /// </summary>
        public override bool SupportsBackNavigation { get; }

        /// <summary>
        /// Determines whether the ability to go back to the previous
        /// navigation item after performing a "go back" operation 
        /// is supported.
        /// </summary>
        public override bool SupportsForwardNavigation { get; }

        /// <summary>
        /// Auto clears the forward stack when performing a direct navigation.
        /// </summary>
        public override bool AutoClearForwardStackOnNavigation { get; }
    }
}
