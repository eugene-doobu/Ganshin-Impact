#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public sealed class PopupDataContext : Context, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _titleText   = string.Empty;
        private string _contentText = string.Empty;
        private bool   _isOkCancel;
        
        public event Action? ClickOkEvent;
        public event Action? ClickCancelEvent;

        [UsedImplicitly]
        public void ClickOk(Context context) => ClickOkEvent?.Invoke();
        [UsedImplicitly]
        public void ClickCancel(Context context) => ClickCancelEvent?.Invoke();

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
        
        [UsedImplicitly]
        public bool IsOkCancel
        {
            get => _isOkCancel;
            set
            {
                _isOkCancel = value;
                OnPropertyChanged();
            }
        }
        
        public void ClearEvent()
        {
            ClickOkEvent     = null;
            ClickCancelEvent = null;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}