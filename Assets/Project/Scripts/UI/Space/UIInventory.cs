using System;
using GanShin.Space.Content;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Space.UI
{
    public class UIInventory : UIRootBase
    {
        [Serializable]
        public class ItemContextHolder
        {
            public ConsumableItemType type;
            public ContextHolder contextHolder;
        }
        
        [Inject]
        private InventoryManager _inventoryManager;
        
        [SerializeField] ItemContextHolder[] itemContextHolders;

        private InventoryContext _context;
        
        protected override Context InitializeDataContext()
        {
            _context = new InventoryContext();
            foreach (ConsumableItemType type in Enum.GetValues(typeof(ConsumableItemType)))
            {
                var itemContext = _context.AddContext(type);
                foreach (var itemContextHolder in itemContextHolders)
                {
                    if (itemContextHolder.type != type) continue;
                    itemContextHolder.contextHolder.Context = itemContext;
                }
            }
            
            var items = _inventoryManager.Items;
            foreach (var kvp in items)
                _context.SetItem(kvp.Key, kvp.Value);
            
            _inventoryManager.OnItemUpdated += OnItemUpdated;

            return _context;
        }

        private void OnItemUpdated(ConsumableItemType type, int avmout)
        {
            _context.SetItem(type, avmout);
        }

        private void OnDestroy()
        {
            _inventoryManager.OnItemUpdated -= OnItemUpdated;
        }
    }
}