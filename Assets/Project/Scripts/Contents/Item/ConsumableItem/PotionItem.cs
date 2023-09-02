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
        }
    }
}