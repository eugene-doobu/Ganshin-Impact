using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class LogItemContext : Context, INotifyPropertyChanged
    {
        private string _content;

        public LogItemContext()
        {
        }

        public LogItemContext(string content)
        {
            Content = content;
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
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