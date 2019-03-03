using Capoala.MVVM;

namespace MvvmPlayground.Services
{
    /// <summary>
    /// A container for storing navigation services.
    /// </summary>
    internal static class NavigationServices
    {
        /// <summary>
        /// The main navigation service.
        /// </summary>
        internal static MvvmNavigator<INotifyPropertyChanges> MainService { get; } = new MvvmNavigator<INotifyPropertyChanges>();
    }
}
