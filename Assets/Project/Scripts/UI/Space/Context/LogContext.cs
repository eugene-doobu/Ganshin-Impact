using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI.Space
{
    [UsedImplicitly]
    public class LogContext : GanContext
    {
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
    }
}