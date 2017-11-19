using Capoala.MVVM;
using DW = System.Diagnostics.Debug;

namespace WPF_Test
{
    //public class ViewModel : Capoala.MVVM.NotifyingObjectBaseSlim
    //{
    //    string _name = null;
    //    public string Name { get => _name; set => SetAndNotify(ref _name, value); }
    //}

    //public class ViewModel : Capoala.MVVM.NotifyingObjectBaseSlim
    //{
    //    string _firstName = null;
    //    public string FirstName
    //    {
    //        get => _firstName;
    //        set
    //        {
    //            _firstName = value;
    //            Notify();
    //            Notify(nameof(FullName));
    //        }
    //    }

    //    string _lastName = null;
    //    public string LastName
    //    {
    //        get => _lastName;
    //        set
    //        {
    //            _lastName = value;
    //            Notify();
    //            Notify(nameof(FullName));
    //        }
    //    }

    //    public string FullName => $"{FirstName} {LastName}";
    //}

    //public class ViewModel : Capoala.MVVM.NotifyingObjectBase
    //{
    //    public string FirstName { get => Get<string>(); set => Set(value); }
    //    public string LastName { get => Get<string>(); set => Set(value); }

    //    [NotifiedOnChange(nameof(FirstName), nameof(LastName))]
    //    public string FullName => $"{FirstName} {LastName}";
    //}

    //public class ViewModel
    //{
    //    public RelayCommand SaveCommand { get; } 
    //        = new RelayCommand(() => DW.WriteLine("Saving..."));

    //    public RelayCommand<string> GetRecordCommand { get; } 
    //        = new RelayCommand<string>(entryName => DW.WriteLine(entryName));
    //}

    //public class ViewModel { }

    //public static class NavigationHelper
    //{
    //    public static INavigationService<object> Navigator { get; } = new DefaultNavigationService();
    //}

    //public class MainViewModel : NotifyingObjectBase
    //{
    //    public object CurrentViewModel { get => Get<object>(); set => Set(value); }

    //    public MainViewModel()
    //    {
    //        NavigationHelper.Navigator.NavigationDidHappen += (s, e) =>
    //        {
    //            CurrentViewModel = NavigationHelper.Navigator.Current;
    //        };

    //        NavigationHelper.Navigator.Navigate(new ViewModel());
    //    }
    //}
}
