using System;
using System.Collections.Generic;
using GanShin.UI;
using JetBrains.Annotations;
using Zenject;

#nullable enable

namespace GanShin.Space.Content
{
    public enum ConsumableItemType
    {
        HP_POTION,
        STAMINA_POTION,
        POTION, // TODO: 아이템 정체성 정하기
    }
    
    [UsedImplicitly]
    public class InventoryManager : IInitializable
    {
        [Inject] public PlayerManager? PlayerManager { get; private set; }
        [Inject] public UIManager?     UIManager     { get; private set; }
        
        private readonly Dictionary<ConsumableItemType, int> _items = new();
        
        public bool IsInitialized { get; private set; }
        
        public event Action<Dictionary<ConsumableItemType, int>>? OnItemUpdated;
        
        public void Initialize()
        {
            LoadItems();
            BindItemToUI();
            IsInitialized = false;
        }

        private void LoadItems()
        {
            // TODO: Load items from save file
            foreach (ConsumableItemType consumableItemType in System.Enum.GetValues(typeof(ConsumableItemType)))
                _items.Add(consumableItemType, 10);
            OnItemUpdated?.Invoke(_items);
        }

        private void BindItemToUI()
        {
        }
    }
}
