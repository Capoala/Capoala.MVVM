using Capoala.MVVM;

namespace MvvmPlayground.Services
{
    internal static class MainNavigationService
    {
        internal static MvvmNavigator<object> Service { get; } = new MvvmNavigator<object>();
    }
}
