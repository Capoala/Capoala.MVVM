using Capoala.MVVM;
using MvvmPlayground.Models;
using System.Collections.Generic;

namespace MvvmPlayground.ViewModels
{
    internal class PersonListingViewModel : NotifyPropertyChangesBaseSlim
    {
        public IEnumerable<Person> People => SharedState.SharedData.People;

        public CommandRelay CreateNewPersonCommand { get; } = new CommandRelay(
            () => Services.MainNavigationService.Service.NavigateTo(SharedState.SharedData.ViewModels.GetCreateNewPersonViewModel));

        public CommandRelay GoBackToLastCreatedPersonCommand { get; } = new CommandRelay(
            () => Services.MainNavigationService.Service.TryGoForward(), 
            () => Services.MainNavigationService.Service.CanGoForward);

        public PersonListingViewModel()
        {
            SharedState.SharedData.People.CollectionChanged += (s, e) => Notify(nameof(People));
            Services.MainNavigationService.Service.NavigationDidHappen += (s, e) => GoBackToLastCreatedPersonCommand?.NotifyCanExecuteDidChange();
        }
    }
}
