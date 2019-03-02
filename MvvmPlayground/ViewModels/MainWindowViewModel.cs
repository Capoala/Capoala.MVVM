using Capoala.MVVM;

namespace MvvmPlayground.ViewModels
{
    internal class MainWindowViewModel : NotifyPropertyChangesBaseAutoBackingStore
    {
        public INotifyPropertyChanges CurrentViewModel { get => Get<INotifyPropertyChanges>(); set => Set(value); }

        public MainWindowViewModel()
        {
            Services.MainNavigationService.Service.NavigationDidHappen += (s, e) => CurrentViewModel = e.NavigationItem;

            Services.MainNavigationService.Service.NavigateTo(SharedState.SharedData.ViewModels.GetPersonListingViewModel.Value);
        }
    }
}
