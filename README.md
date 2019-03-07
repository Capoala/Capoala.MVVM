# Capoala.MVVM
`Capoala.MVVM` is a minimalist framework for MVVM based applications. This framework provides both IPropertyChanged and ICommand implementations, as well as a simple navigation base.


# ICommand
## CommandRelay
The `CommandRelay` implements the ICommand interface and comes in two flavors; `CommandRelay` and `CommandRelay<TExecutionParameter>` for passing a parameter to the execute method.

#### Class ViewModel
```csharp
using Capoala.MVVM;
using System.Diagnostics;

internal class ViewModel
{
    public CommandRelay SaveCommand { get; } 
        = new CommandRelay(() => Debug.WriteLine("Saving..."));

    public CommandRelay<string> AddInputCommand { get; } 
        = new CommandRelay<string>(input => Debug.WriteLine(input));
}
```
#### XAML
```xml
...

<!-- Bind to SaveCommand -->
<Button Content="Save" 
        Command="{Binding SaveCommand}"/>

...

<!-- Bind to AddInputCommand using a parameter -->
<Button Content="Add" 
        Command="{Binding AddInputCommand}", 
        CommandParameter="{Binding ElementName=InputTextBox, Path=Text}"/>

...
```

# INotifyPropertyChanged
## INotifyPropertyChanges
The `INotifyPropertyChanges` is a custom interface for implementing the `INotifyPropertyChanged` interface. The `INotifyPropertyChanges` interface exposes methods for implementing the `INotifyPropertyChanged` event and is defined - without summaries - as follows:

```csharp
public interface INotifyPropertyChanges : INotifyPropertyChanged
{
    void Notify([CallerMemberName] string propertyName = null);

    bool SetAndNotify<T>(
        ref T backingField, 
        T value, 
        EqualityComparer<T> equalityComparer = null, 
        [CallerMemberName] string propertyName = null);
}
```
## NotifyPropertyChangesBaseSlim
`NotifyPropertyChangesBaseSlim` is an abstract class that simply implements the `INotifyPropertyChanges` interface.

## NotifyPropertyChangesBase
`NotifyPropertyChangesBase` is an abstract class that implements the `INotifyPropertyChanges` interface and supports the `SubscribeToChanges`, `NotifyOnChange`, and `CanExecuteDependentOn` attributes. These attributes allow a streamlined approach to notifying other properties and re-triggering the `NotifyCanExecuteDidChange` method on `CommandRelay` objects.

```csharp
using Capoala.MVVM;
using System.Windows;

internal class SampleViewModel : NotifyPropertyChangesBase
{
    private bool _isOperationInProgress = false;
    public bool IsOperationInProgress 
    { 
        get => _isOperationInProgress; 
        set => SetAndNotify(ref _isOperationInProgress, value); 
    }

    private string _firstName;
    public string FirstName 
    { 
        get => _firstName; 
        set => SetAndNotify(ref _firstName, value); 
    }

    private string _lastName;
    public string LastName 
    { 
        get => _lastName; 
        set => SetAndNotify(ref _lastName, value); 
    }

    [SubscribeToChanges(nameof(FirstName), nameof(LastName))]
    public string DisplayName => string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)
        ? null
        : string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName)
        ? $"{FirstName}"
        : !string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)
        ? $"{LastName}"
        : $"{LastName}, {FirstName}";

    [CanExecuteDependentOn(nameof(FirstName), nameof(LastName), nameof(IsOperationInProgress))]
    public CommandRelay CreateCommand { get; }

    public SampleViewModel() => CreateCommand = new CommandRelay(() => 
    {
        // Do Something.

    }, () => !IsOperationInProgress && 
             !string.IsNullOrEmpty(FirstName) && 
             !string.IsNullOrEmpty(LastName))
}
```

## NotifyPropertyChangesBaseAutoBackingStore
`NotifyPropertyChangesBaseAutoBackingStore` is an abstract class that inherits from `NotifyPropertyChangesBase`, but includes a "backing store" in the form of a `Dictionary<string, object>`. This allows us to not have to define private fields and is useful for testing and "notifying" models. Using the above sample, below is how the code would look using the `NotifyPropertyChangesBaseAutoBackingStore` base class.

```csharp
using Capoala.MVVM;
using System.Windows;

internal class SampleViewModel : NotifyPropertyChangesBaseAutoBackingStore
{
    public bool IsOperationInProgress { get => Get<bool>(); set => Set(value); 
    public string FirstName { get => Get<string>(); set => Set(value); 
    public string LastName { get => Get<string>(); set => Set(value); 

    [SubscribeToChanges(nameof(FirstName), nameof(LastName))]
    public string DisplayName => string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)
        ? null
        : string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName)
        ? $"{FirstName}"
        : !string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(FirstName)
        ? $"{LastName}"
        : $"{LastName}, {FirstName}";

    [CanExecuteDependentOn(nameof(FirstName), nameof(LastName), nameof(IsOperationInProgress))]
    public CommandRelay CreateCommand { get; }

    public SampleViewModel() => CreateCommand = new CommandRelay(() => 
    {
        // Do Something.

    }, () => !IsOperationInProgress && 
             !string.IsNullOrEmpty(FirstName) && 
             !string.IsNullOrEmpty(LastName))    
}
```

# Navigation
## SimpleMvvmNavigator<TNavigationItem>
The `SimpleMvvmNavigator<TNavigationItem>` class is a simplistic implementation for navigation. This class exposes only an event for subscribing to navigation changes and a method for performing the navigation itself. 

```csharp
internal static class Services
{
    internal static SimpleMvvmNavigator<INotifyPropertyChanges> Navigator { get; } 
        = new SimpleMvvmNavigator<INotifyPropertyChanges>();
}

internal class MainViewModel : NotifyPropertyChangesBaseAutoBackingStore
{
    public INotifyPropertyChanges CurrentViewModel 
    { 
        get => Get<INotifyPropertyChanges>(); 
        set => Set(value); 
    }

    public MainViewModel() => 
        Services.Navigator.NavigationDidHappen += (s, e) => 
            CurrentViewModel = e.NavigationItem;
}

internal class TestViewModel : INotifyPropertyChanges
{
    public CommandRelay<INotifyPropertyChanges> Navigate { get; } 
        = new CommandRelay<INotifyPropertyChanges>(
            (viewModel) => Services.Navigator.NavigateTo(viewModel));
}
```

## MvvmNavigatorBase<TNavigationItem>
`MvvmNavigatorBase<TNavigationItem>` is an abstract class for implementing a more robust, full featured navigation scheme. This class has default support for going both back and forward as well as additional properties to allow for modification on class inheritance.

## MvvmNavigator
`MvvmNavigator` is a default implementation of `MvvmNavigatorBase` and allows constructor settings for supporting back and forward navigation, which can yield a small performance benefit.

```csharp
internal static class Services
{
    internal static MvvmNavigator<INotifyPropertyChanges> Navigator { get; } 
        = new MvvmNavigator<INotifyPropertyChanges>();

    internal static MvvmNavigator<INotifyPropertyChanges> NoForwardNavigator { get; } 
        = new MvvmNavigator<INotifyPropertyChanges>(supportsForwardNavigation: false);

    internal static MvvmNavigator<INotifyPropertyChanges> NoBackNavigator { get; } 
        = new MvvmNavigator<INotifyPropertyChanges>(supportsBackNavigation: false);

    // This is essentially the same as "SimpleMvvmNavigator", but still exposes
    // properties and methods for going back and forward, as we are still
    // inheriting from the base class.     
    internal static MvvmNavigator<INotifyPropertyChanges> NoBackNavigator { get; } 
        = new MvvmNavigator<INotifyPropertyChanges>(
            supportsBackNavigation: false, 
            supportsForwardNavigation: false);
}
```

## Attributes
### SubscribeToChanges
`SubscribeToChanges` is an attribute used to allow a property with this attribute to subscribe to value changes of other properties.

### NotifyOnChange
`NotifyOnChange` is an attribute used to allow a property with this attribute to notify other properties when its value changes.

### CanExecuteDependentOn
`CanExecuteDependentOn` is an attribute used to allow a property - of type `CommandRelay` or `CommandRelay<TExecutionParameter>` - to subscribe to value changes of other properties. This attribute differs from `SubscribeToChanges`, as `SubscribeToChanges` will raise the `PropertyChangedEventHandler` event of an `INotifyPropertyChanged` interface, while this attribute will raise the `CanExecuteChanged` event of an `ICommand` interface.

## Events and Event Arguments
### NavigationChangedEventHandler<TNavigationItem>
`NavigationChangedEventHandler<TNavigationItem>` is a delegate for supporting navigation changes.

### NavigationChangedEventArgs<TNavigationItem>
The `NavigationChangedEventArgs<TNavigationItem>` class is used with the `NavigationChangedEventHandler<TNavigationItem>` event handler for navigation support.
