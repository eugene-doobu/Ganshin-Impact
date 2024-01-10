namespace GanShin.Space.Content
{
    public class HpPotionItem : ConsumableItem
    {
        private readonly float _healAmount = 25;

        public HpPotionItem(InventoryManager owner) : base(owner)
        {
            Type = ConsumableItemType.HP_POTION;
        }

        public override void Use()
        {
            var currentPlayer = Owner.PlayerManager?.CurrentPlayer;
            if (currentPlayer == null)
                return;

            currentPlayer.OnHealed(_healAmount);
            (this as IUILoggable).UILog("체력포션을 사용하여 체력을 회복하였습니다", Owner.UIManager);
        }
    }
}