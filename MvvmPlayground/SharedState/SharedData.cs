using MvvmPlayground.Models;
using MvvmPlayground.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace MvvmPlayground.SharedState
{
    /// <summary>
    /// A container for shared state data.
    /// </summary>
    internal static class SharedData
    {
        /// <summary>
        /// A collection of people.
        /// </summary>
        internal static ObservableCollection<Person> People { get; } = new ObservableCollection<Person>();

        /// <summary>
        /// View models.
        /// </summary>
        internal static class ViewModels
        {
            /// <summary>
            /// Creates and returns a new instance of <see cref="CreateNewPersonViewModel"/>.
            /// </summary>
            internal static CreateNewPersonViewModel GetCreateNewPersonViewModel => new CreateNewPersonViewModel();

            /// <summary>
            /// Returns the value of the lazy singleton for <see cref="PersonListingViewModel"/>.
            /// </summary>
            internal static Lazy<PersonListingViewModel> GetPersonListingViewModel { get; } = new Lazy<PersonListingViewModel>();
        }
    }
}
