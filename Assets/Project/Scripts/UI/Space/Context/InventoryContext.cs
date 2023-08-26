using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GanShin.Space.Content;
using GanShin.UI;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class InventoryContext : Context, INotifyPropertyChanged
    {
        private readonly Dictionary<ConsumableItemType, Context> _items = new();

        public event PropertyChangedEventHandler  PropertyChanged;
        
        public void AddContext(ConsumableItemType type)
        {
            _items.Add(type, new InventoryItemContext());
        }
        
        public void RemoveContext(ConsumableItemType type)
        {
            if (_items.TryGetValue(type, out var context))
                (context as IDisposable)?.Dispose();
            
            _items.Remove(type);
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
