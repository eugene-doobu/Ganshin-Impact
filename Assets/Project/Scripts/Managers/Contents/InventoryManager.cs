using System;
using System.Collections.Generic;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;
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
    public class InventoryManager : IInitializable, ITickable
    {
        [Inject] public PlayerManager? PlayerManager { get; private set; }
        [Inject] public UIManager?     UIManager     { get; private set; }
        
        private readonly Dictionary<ConsumableItemType, int> _itemAmount = new();
        private readonly Dictionary<ConsumableItemType, ConsumableItem> _items = new();
        
        public IReadOnlyDictionary<ConsumableItemType, int> ItemAmount => _itemAmount;
        public bool IsInitialized { get; private set; }
        
        public event Action<ConsumableItemType, int>? OnItemAmountUpdated;
        
        public void Initialize()
        {
            InitializeItems();
            LoadItems();
            IsInitialized = false;
        }
        
        private void InitializeItems()
        {
            // 어셈블리를 읽어 자동 셋팅해줄 수 있으나, 굳이 그렇게 구현하진 않겠습니다
            _items.Add(ConsumableItemType.HP_POTION, new HpPotionItem(this));
            _items.Add(ConsumableItemType.STAMINA_POTION, new StaminaPotionItem(this));
            _items.Add(ConsumableItemType.POTION, new PotionItem(this));
        }

        private void LoadItems()
        {
            // TODO: Load items from save file
            foreach (ConsumableItemType consumableItemType in Enum.GetValues(typeof(ConsumableItemType)))
                _itemAmount.Add(consumableItemType, 10);
            foreach (var kvp in _itemAmount)
                OnItemAmountUpdated?.Invoke(kvp.Key, kvp.Value);
        }

        public void Tick()
        {
            if (Input.GetKeyDown("1"))
                UseItem(ConsumableItemType.HP_POTION);
            
            if (Input.GetKeyDown("2"))
                UseItem(ConsumableItemType.STAMINA_POTION);

            if (Input.GetKeyDown("3"))
                UseItem(ConsumableItemType.POTION);
        }
        
        private void ItemAmountUpdated(ConsumableItemType type, int amount)
        {
            _itemAmount[type] = Mathf.Max(amount, 0);;
            OnItemAmountUpdated?.Invoke(type, amount);
        }

        private void UseItem(ConsumableItemType type)
        {
            if (!_itemAmount.ContainsKey(type)) return;
            if (_itemAmount[type] <= 0)
            {
                UIManager?.SetToast("아이템 사용불가", "아이템을 부족합니다", EToastType.WARNING);
                return;
            }
            ItemAmountUpdated(type, _itemAmount[type] - 1);
            OnItemAmountUpdated?.Invoke(type, _itemAmount[type]);
            
            if (!_items.TryGetValue(type, out var item)) return;
            item.Use();
        }
    }
}
