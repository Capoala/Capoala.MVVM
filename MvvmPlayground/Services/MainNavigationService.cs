using Capoala.MVVM;

namespace MvvmPlayground.Services
{
    internal static class MainNavigationService
    {
        internal static MvvmNavigator<INotifyPropertyChanges> Service { get; } = new MvvmNavigator<INotifyPropertyChanges>();
    }
}
