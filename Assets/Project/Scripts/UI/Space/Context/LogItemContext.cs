using GanShin.UI;
using JetBrains.Annotations;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class LogItemContext : GanContext
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
    }
}