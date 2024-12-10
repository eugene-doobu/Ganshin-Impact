using JetBrains.Annotations;

namespace GanShin.UI.Space
{
    public class InventoryItemContext : GanContext
    {
        private int _amount;

        [UsedImplicitly]
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public void Dispose()
        {
            Amount = 0;
        }
    }
}