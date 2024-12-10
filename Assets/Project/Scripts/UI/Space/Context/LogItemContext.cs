using JetBrains.Annotations;

namespace GanShin.UI.Space
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