namespace GanShin.Space.Content
{
    public class StaminaPotionItem : ConsumableItem
    {
        private float _healAmount = 25;
        
        public override void Use()
        {
            var playerManager = Owner.PlayerManager;
            if (playerManager == null)
                return;
            
            playerManager.CurrentStamina += _healAmount;
        }
        
        public override void Initialize(InventoryManager owner)
        {
            base.Initialize(owner);
            Type = ConsumableItemType.STAMINA_POTION;
        }
    }
}