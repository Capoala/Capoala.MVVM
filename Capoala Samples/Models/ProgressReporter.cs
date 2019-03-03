using Capoala.MVVM;

namespace Capoala_Samples.Models
{
    internal class ProgressReporter : NotifyPropertyChangesBaseAutoBackingStore
    {
        public string Status { get => Get<string>(); set => Set(value); }
        public double CurrentProgressComplete { get => Get<double>(); set => Set(value); }
    }
}
