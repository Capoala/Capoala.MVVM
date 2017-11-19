# Capoala.MVVM
`Capoala.MVVM` is a featherlight MVVM framework designed for simple-to-moderate MVVM design pattern applications. This framework includes basic implementations for handling property notifications, command bindings, and navigation.

# INotifyPropertyChanged
## NotifyingObjectBaseSlim
The `NotifyingObjectBaseSlim` class is a slim, standard `INotifyPropertyChanged` implementation. A streamlined method named `SetAndNotify()` will set the referenced property's value and raise the `PropertyChanged` event in one swell swoop.
```csharp
public class ViewModel : Capoala.MVVM.NotifyingObjectBaseSlim
{
    string _name = null;
    public string Name { get => _name; set => SetAndNotify(ref _name, value); }
}
```
In cases where extra work or additional notifications are required, you can use standard getter setter syntax and utilize the `Notify()` method to raise the `PropertyChanged` event on the property being set.
```csharp
public class ViewModel : Capoala.MVVM.NotifyingObjectBaseSlim
{
    string _firstName = null;
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            Notify();
            Notify(nameof(FullName));
        }
    }

    string _lastName = null;
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            Notify();
            Notify(nameof(FullName));
        }
    }

    public string FullName => $"{FirstName} {LastName}";
}
```
## NotifyingObjectBase
The `NotifyingObjectBase` class is a more feature-rich implementation of the `INotifyPropertyChanged` interface. This class removes the requirement of a backing field and moves it into a backing store in the form of a `Dictionary<string, object>`. 

This class also provides an attribute approach to streamlining property notification dependencies. In the below example, the property `FullName` subscribes to the `FirstName` and `LastName` event changes. When either `FirstName` or `LastName` is changed, `FullName` is also notified.
```csharp
public class ViewModel : Capoala.MVVM.NotifyingObjectBase
{
    public string FirstName { get => Get<string>(); set => Set(value); }
    public string LastName { get => Get<string>(); set => Set(value); }

    [NotifiedOnChange(nameof(FirstName), nameof(LastName))]
    public string FullName => $"{FirstName} {LastName}";
}
```

# ICommand
## RelayCommand
The `RelayCommand` implements the ICommand interface and comes in two flavors; `RelayCommand` and `RelayCommand<T>` for passing command paramters.

```csharp
using Capoala.MVVM;
using DW = System.Diagnostics.Debug;

public class ViewModel
{
    public RelayCommand SaveCommand { get; } 
        = new RelayCommand(() => DW.WriteLine("Saving..."));

    public RelayCommand<string> GetRecordCommand { get; } 
        = new RelayCommand<string>(entryName => DW.WriteLine(entryName));
}
```
```xml
...
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition/>
        <ColumnDefinition Width="Auto"/>
    </Grid.ColumnDefinitions>
    <TextBox x:Name="RecordID"/>
    <Button Grid.Column="1" 
            Margin="10 0 0 0" 
            Command="{Binding GetRecordCommand}" 
            CommandParameter="{Binding ElementName=RecordID, Path=Text}">Retreive</Button>
</Grid>
<Grid Grid.Row="1" Margin="0 20 0 0">
    <!--Content-->
</Grid>
<Grid Grid.Row="2" Margin="0 20 0 0">
    <Button HorizontalAlignment="Right" 
            Command="{Binding SaveCommand}">Save</Button>
</Grid>
...
```

# Navigation
A simplistic navigation implementation is also available which should satisfy most requirements. The navigation system includes an `INavigationService<TNavigationItem>` interface, and a `NavigationServiceBase<TNavigationItem>` base class.

```csharp
using Capoala.MVVM;

public static class NavigationHelper
{
    public static INavigationService<object> Navigator { get; } 
        = new DefaultNavigationService();
}

public class ViewModel { }

public class MainViewModel : NotifyingObjectBase
{
    public object CurrentViewModel { get => Get<object>(); set => Set(value); }

    public MainViewModel()
    {
        NavigationHelper.Navigator.NavigationDidHappen += (s, e) =>
        {
            CurrentViewModel = NavigationHelper.Navigator.Current;
        };

        NavigationHelper.Navigator.Navigate(new ViewModel());
    }
}
```