using System.ComponentModel;

namespace RDPQuickAccess.ViewModel
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChaged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected readonly App app = ((App)App.Current);
    }
}
