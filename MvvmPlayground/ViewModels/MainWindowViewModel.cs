using Capoala.MVVM;

namespace MvvmPlayground.ViewModels
{
    /// <summary>
    /// The main view model for the main window.
    /// </summary>
    internal class MainWindowViewModel : NotifyPropertyChangesBaseAutoBackingStore
    {
        /// <summary>
        /// The current view model.
        /// </summary>
        public INotifyPropertyChanges CurrentViewModel { get => Get<INotifyPropertyChanges>(); set => Set(value); }

        /// <summary>
        /// Creates a new <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            // Update the current view model upon navigation change.
            Services.NavigationServices.MainService.NavigationDidHappen += (s, e) => CurrentViewModel = e.NavigationItem;

            // Navigate to a navigation item.
            Services.NavigationServices.MainService.NavigateTo(SharedState.SharedData.ViewModels.GetPersonListingViewModel.Value);
        }
    }
}
