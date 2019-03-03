using Capoala.MVVM;

namespace Capoala_Samples.Models
{
    internal class MessageModel : NotifyPropertyChangesBaseAutoBackingStore
    {
        public string Message { get => Get<string>(); set => Set(value); }
    }
}
