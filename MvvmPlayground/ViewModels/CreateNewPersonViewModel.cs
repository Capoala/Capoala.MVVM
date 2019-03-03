using Capoala.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmPlayground.ViewModels
{
    /// <summary>
    /// A view mode for creating a new person.
    /// </summary>
    internal class CreateNewPersonViewModel : NotifyPropertyChangesBase
    {
        private bool _isOperationInProgress = false;
        private string _firstName;
        private string _lastName;


        /// <summary>
        /// Determines whether an operation is currently in progress.
        /// </summary>
        public bool IsOperationInProgress { get => _isOperationInProgress; set => SetAndNotify(ref _isOperationInProgress, value); }

        /// <summary>
        /// The first name of the person.
        /// </summary>
        public string FirstName { get => _firstName; set => SetAndNotify(ref _firstName, value); }

        /// <summary>
        /// The last name of the person.
        /// </summary>
        public string LastName { get => _lastName; set => SetAndNotify(ref _lastName, value); }

        /// <summary>
        /// The display name as presented on official documents.
        /// </summary>
        /// <remarks>
        /// Note that this property utilizes the <see cref="SubscribeToChanges"/> attribute.
        /// This means that when <see cref="FirstName"/>, <see cref="MiddleName"/> or <see cref="LastName"/>
        /// changed, this property also notifies the owner that it has changed.
        /// </remarks>
        [SubscribeToChanges(nameof(FirstName), nameof(LastName))]
        public string DisplayName => string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)
            ? null
            : string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName) 
            ? $"{FirstName}"
            : !string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)
            ? $"{LastName}"
            : $"{LastName}, {FirstName}";


        /// <summary>
        /// The command which creates a new person.
        /// </summary>
        /// <remarks>
        /// Note that this property utilizes the <see cref="SubscribeToChanges"/> attribute.
        /// This means that when <see cref="FirstName"/>, <see cref="LastName"/> or
        /// <see cref="IsOperationInProgress"/> changes, the method <see cref="CommandRelay.NotifyCanExecuteDidChange"/>
        /// is executed for this property.
        /// </remarks>
        [CanExecuteDependentOn(nameof(FirstName), nameof(LastName), nameof(IsOperationInProgress))]
        public CommandRelay<object> CreateCommand { get; }

        /// <summary>
        /// Cancels the operation by going back to the previous page.
        /// </summary>
        public CommandRelay CancelCommand { get; } = new CommandRelay(() => Services.MainNavigationService.Service.TryGoBack());

        /// <summary>
        /// Creates a new <see cref="CreateNewPersonViewModel"/> instance.
        /// </summary>
        public CreateNewPersonViewModel() => CreateCommand = new CommandRelay<object>((obj) =>
        {
            // This is the work that will be done when called.
            IsOperationInProgress = true;
            SharedState.SharedData.People.Add(new Models.Person() { FirstName = FirstName, LastName = LastName });
            MessageBox.Show($"{FirstName} {LastName} has been created!", DisplayName, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            IsOperationInProgress = false;
            Services.MainNavigationService.Service.NavigateTo(SharedState.SharedData.ViewModels.GetPersonListingViewModel.Value);

            // We'll add the logic for determining whether the command is enabled or not. 
            // Notice we use the same property names with the attribute for this property.
            // This gives us a nice pairing and clean code.
        }, () => !IsOperationInProgress && !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName));
    }
}
