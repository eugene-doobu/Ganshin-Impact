using System;
using System.Collections.Generic;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;

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
    public class InventoryManager : ManagerBase
    {
        [UsedImplicitly] public InventoryManager() { }
        
        public PlayerManager? PlayerManager => ProjectManager.Instance.GetManager<PlayerManager>();
        public UIManager?     UIManager     => ProjectManager.Instance.GetManager<UIManager>();
        
        private readonly Dictionary<ConsumableItemType, int> _itemAmount = new();
        private readonly Dictionary<ConsumableItemType, ConsumableItem> _items = new();

        private int _gold;
        
        public IReadOnlyDictionary<ConsumableItemType, int> ItemAmount => _itemAmount;
        
        public int Gold
        {
            get => _gold;
            set
            {
                _gold = value;
                OnGoldUpdated?.Invoke(_gold);
            }
        }
        
        public event Action<int>? OnGoldUpdated;

        public event Action<ConsumableItemType, int>? OnItemAmountUpdated;
        
        public override void Initialize()
        {
            InitializeGold();
            InitializeItems();
            LoadItems();
        }

        public override void Tick()
        {
            if (Input.GetKeyDown("1"))
                UseItem(ConsumableItemType.HP_POTION);
            
            if (Input.GetKeyDown("2"))
                UseItem(ConsumableItemType.STAMINA_POTION);

            if (Input.GetKeyDown("3"))
                UseItem(ConsumableItemType.POTION);
        }

        private void InitializeGold()
        {
            // TODO: LoadAsset items from save file
            Gold = 1000;
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
            // TODO: LoadAsset items from save file
            foreach (ConsumableItemType consumableItemType in Enum.GetValues(typeof(ConsumableItemType)))
                _itemAmount.Add(consumableItemType, 10);
            foreach (var kvp in _itemAmount)
                OnItemAmountUpdated?.Invoke(kvp.Key, kvp.Value);
        }
        
        private void ItemAmountUpdated(ConsumableItemType type, int amount)
        {
            _itemAmount[type] = Mathf.Max(amount, 0);;
            OnItemAmountUpdated?.Invoke(type, amount);
        }

        private void UseItem(ConsumableItemType type)
        {
            if (!_itemAmount.ContainsKey(type)) return;

            var currentPlayer = PlayerManager?.CurrentPlayer;
            if (currentPlayer == null || currentPlayer.IsDead)
            {
                UIManager?.SetToast("아이템 사용불가", "플레이어가 사망하였습니다.", EToastType.ERROR);
                return;
            }
            
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
