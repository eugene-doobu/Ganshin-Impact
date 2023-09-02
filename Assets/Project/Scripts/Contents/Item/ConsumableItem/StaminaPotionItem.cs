namespace GanShin.Space.Content
{
    public class StaminaPotionItem : ConsumableItem
    {
        private float _healAmount = 25;
        
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
        }
    }
}