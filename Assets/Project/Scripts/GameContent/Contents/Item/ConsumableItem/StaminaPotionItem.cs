namespace GanShin.Space.Content
{
    public class StaminaPotionItem : ConsumableItem
    {
        private readonly float _healAmount = 25;

        public StaminaPotionItem(InventoryManager owner) : base(owner)
        {
            Type = ConsumableItemType.STAMINA_POTION;
        }

        public override void Use()
        {
            var playerManager = Owner.PlayerManager;
            if (playerManager == null)
                return;

            playerManager.CurrentStamina += _healAmount;
            (this as IUILoggable).UILog("스테미나 포션을 사용하였습니다.", Owner.UIManager);
        }
    }
}