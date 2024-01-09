using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class ToastPopupDataContext : Context, INotifyPropertyChanged
    {
        private string _toastContent;

        private string _toastTitle;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}