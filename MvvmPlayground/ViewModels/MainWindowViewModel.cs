using Capoala.MVVM;

namespace MvvmPlayground.ViewModels
{
    internal class MainWindowViewModel : NotifyPropertyChangedBaseAutoBackingStore
    {
        public object CurrentViewModel { get => Get<object>(); set => Set(value); }

        public MainWindowViewModel()
        {
            Services.MainNavigationService.Service.NavigationDidHappen += (s, e) => CurrentViewModel = e.NavigationItem;

            Services.MainNavigationService.Service.NavigateTo(SharedState.SharedData.ViewModels.GetPersonListingViewModel.Value);
        }
    }
}
