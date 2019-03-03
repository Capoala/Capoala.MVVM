using Capoala.MVVM;
using MvvmPlayground.Models;
using System.Collections.Generic;

namespace MvvmPlayground.ViewModels
{
    /// <summary>
    /// A view model for listing and creating new people.
    /// </summary>
    internal class PersonListingViewModel : NotifyPropertyChangesBaseSlim
    {
        /// <summary>
        /// The created people.
        /// </summary>
        public IEnumerable<Person> People => SharedState.SharedData.People;

        /// <summary>
        /// Navigates to a new view where a person can be created.
        /// </summary>
        public CommandRelay CreateNewPersonCommand { get; } = new CommandRelay(
            () => Services.NavigationServices.MainService.NavigateTo(SharedState.SharedData.ViewModels.GetCreateNewPersonViewModel));

        /// <summary>
        /// Returns to the previous person creation view.
        /// </summary>
        public CommandRelay GoBackToLastCreatedPersonCommand { get; } = new CommandRelay(
            () => Services.NavigationServices.MainService.TryGoForward(),
            () => Services.NavigationServices.MainService.CanGoForward);

        /// <summary>
        /// Creates a new instance of <see cref="PersonListingViewModel"/>.
        /// </summary>
        public PersonListingViewModel()
        {
            // Notify our local property when the shared state collection changes.
            SharedState.SharedData.People.CollectionChanged += (s, e) => Notify(nameof(People));

            // When navigation happens, notify commands that their enabled state may have changed.
            Services.NavigationServices.MainService.NavigationDidHappen += (s, e) => GoBackToLastCreatedPersonCommand?.NotifyCanExecuteDidChange();
        }
    }
}
