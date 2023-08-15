using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace GanShin.UI
{
    public sealed class PopupDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _titleText;
        private string _contentText;

        [UsedImplicitly]
        public string TitleText
        {
            get => _titleText;
            set
            {
                _titleText = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public string ContentText
        {
            get => _contentText;
            set
            {
                _contentText = value;
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