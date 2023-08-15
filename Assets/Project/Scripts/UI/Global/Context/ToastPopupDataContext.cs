using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace GanShin.UI
{
    public class ToastPopupDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _toastTitle;
        private string _toastContent;
        
        [UsedImplicitly]
        public string ToastTitle
        {
            get => _toastTitle;
            set
            {
                _toastTitle = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public string ToastContent
        {
            get => _toastContent;
            set
            {
                _toastContent = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
