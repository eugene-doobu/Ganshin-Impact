namespace GanShin.Space.Content
{
    public class PotionItem : ConsumableItem
    {
        public PotionItem(InventoryManager owner) : base(owner)
        {
            Type = ConsumableItemType.POTION;
        }

        public override void Use()
        {
            // TODO
            (this as IUILoggable).UILog("포션을 사용하였습니다.", Owner.UIManager);
        }
    }
}