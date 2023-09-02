namespace GanShin.Space.Content
{
    public class HpPotionItem : ConsumableItem
    {
        public HpPotionItem(InventoryManager owner) : base(owner)
        {
            Type = ConsumableItemType.HP_POTION;
        }

        private float _healAmount = 25;
        
        public override void Use()
        {
            var currentPlayer = Owner.PlayerManager?.CurrentPlayer;
            if (currentPlayer == null)
                return;
            
            currentPlayer.OnHealed(_healAmount);
        }
    }
}