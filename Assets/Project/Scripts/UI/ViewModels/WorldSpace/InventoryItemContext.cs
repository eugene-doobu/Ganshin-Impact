using System;
using JetBrains.Annotations;

namespace GanShin.UI.ViewModels.WorldSpace
{
    public class InventoryItemContext : GanContext, IDisposable
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