namespace GanShin.Space.Content
{
    public class HpPotionItem : ConsumableItem
    {
        private float _healAmount = 25;
        
        public override void Use()
        {
            var currentPlayer = Owner.PlayerManager?.CurrentPlayer;
            if (currentPlayer == null)
                return;
            
            currentPlayer.OnHealed(_healAmount);
        }
        
        public override void Initialize(InventoryManager owner)
        {
            base.Initialize(owner);
            Type = ConsumableItemType.HP_POTION;
        }
    }
}