using JetBrains.Annotations;

namespace GanShin.UI
{
    public class ToastPopupDataContext : GanContext
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
    }
}