using System;
using GanShin.Space.Content;
using Slash.Unity.DataBind.Core.Data;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace GanShin.UI.Space
{
    public class UIInventory : UIRootBase
    {
        [SerializeField] private ItemContextHolder[] itemContextHolders;

        private readonly InventoryManager _inventoryManager = ProjectManager.Instance.GetManager<InventoryManager>();

        private InventoryContext _context;

        private void OnDestroy()
        {
            _inventoryManager.OnGoldUpdated       -= OnGoldUpdated;
            _inventoryManager.OnItemAmountUpdated -= OnItemAmountUpdated;
        }

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

            var items = _inventoryManager.ItemAmount;
            foreach (var kvp in items)
                _context.SetItem(kvp.Key, kvp.Value);

            _context.Gold = _inventoryManager.Gold;

            _inventoryManager.OnGoldUpdated       += OnGoldUpdated;
            _inventoryManager.OnItemAmountUpdated += OnItemAmountUpdated;

            return _context;
        }

        private void OnGoldUpdated(int gold)
        {
            _context.Gold = gold;
        }

        private void OnItemAmountUpdated(ConsumableItemType type, int avmout)
        {
            _context.SetItem(type, avmout);
        }

        [Serializable]
        public class ItemContextHolder
        {
            public ConsumableItemType type;
            public ContextHolder      contextHolder;
        }
    }
}