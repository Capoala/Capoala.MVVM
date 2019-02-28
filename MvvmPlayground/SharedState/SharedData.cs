using MvvmPlayground.Models;
using MvvmPlayground.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace MvvmPlayground.SharedState
{
    internal static class SharedData
    {
        internal static ObservableCollection<Person> People { get; } = new ObservableCollection<Person>();

        internal static class ViewModels
        {
            internal static CreateNewPersonViewModel GetCreateNewPersonViewModel => new CreateNewPersonViewModel();
            internal static Lazy<PersonListingViewModel> GetPersonListingViewModel { get; } = new Lazy<PersonListingViewModel>();
        }
    }
}
