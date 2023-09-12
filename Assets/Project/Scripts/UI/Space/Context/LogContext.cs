using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class LogContext : Context, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Collection<LogItemContext> _itemsProperty = new();

        public Collection<LogItemContext> Items
        {
            get => _itemsProperty;
            set
            {
                _itemsProperty = value;
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
