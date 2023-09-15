using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    public class InteractionContext : GanContext
    {
        private Collection<InteractionItemContext> _itemsProperty = new();

        public Collection<InteractionItemContext> Items
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