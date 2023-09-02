using System;
using System.Collections.Generic;
using GanShin.Space.Content;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class InventoryContext : Context
    {
        private readonly Dictionary<ConsumableItemType, InventoryItemContext> _items = new();

        public Context AddContext(ConsumableItemType type)
        {
            var context = new InventoryItemContext();
            _items.Add(type, context);
            return context;
        }
        
        public void RemoveContext(ConsumableItemType type)
        {
            if (_items.TryGetValue(type, out var context))
                (context as IDisposable)?.Dispose();
            
            _items.Remove(type);
        }

        public void SetItem(ConsumableItemType type, int value)
        {
            if (!_items.TryGetValue(type, out var context)) return;
            
            context.Amount = value;
        }
    }
}
