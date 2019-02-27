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
    internal class CreateNewPersonViewModel : NotifyPropertyChangedBase
    {
        private bool _isOperationInProgress = false;
        private string _firstName;
        private string _lastName;
        private string _middleName;


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
        /// The middle name of the person.
        /// </summary>
        public string MiddleName { get => _middleName; set => SetAndNotify(ref _middleName, value); }

        /// <summary>
        /// The display name as presented on official documents.
        /// </summary>
        /// <remarks>
        /// Note that this property utilizes the <see cref="SubscribeToChanges"/> attribute.
        /// This means that when <see cref="FirstName"/>, <see cref="MiddleName"/> or <see cref="LastName"/>
        /// changed, this property also notifies the owner that it has changed.
        /// </remarks>
        [SubscribeToChanges(nameof(FirstName), nameof(MiddleName), nameof(LastName))]
        public string DisplayName => $"{LastName}, {FirstName} {MiddleName?.FirstOrDefault()}".Trim();


        /// <summary>
        /// The command which creates a new person.
        /// </summary>
        /// <remarks>
        /// Note that this property utilizes the <see cref="SubscribeToChanges"/> attribute.
        /// This means that when <see cref="FirstName"/>, <see cref="LastName"/> or
        /// <see cref="IsOperationInProgress"/> changes, the method <see cref="CommandRelay.NotifyCanExecuteDidChange"/>
        /// is executed for this property.
        /// </remarks>
        [SubscribeToChanges(nameof(FirstName), nameof(LastName), nameof(IsOperationInProgress))]
        public CommandRelay CreateCommand { get; }


        /// <summary>
        /// Creates a new <see cref="CreateNewPersonViewModel"/> instance.
        /// </summary>
        internal CreateNewPersonViewModel() => CreateCommand = new Capoala.MVVM.CommandRelay(() =>
        {
            // This is the work that will be done when called.
            IsOperationInProgress = true;
            MessageBox.Show($"{FirstName} {LastName} has been created!", DisplayName, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            IsOperationInProgress = false;

            // We'll add the logic for determining whether the command is enabled or not. 
            // Notice we use the same property names with the attribute for this property.
            // This gives us a nice pairing and clean code.
        }, () => !IsOperationInProgress && !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName));
    }
}
