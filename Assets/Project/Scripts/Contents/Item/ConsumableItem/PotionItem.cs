namespace GanShin.Space.Content
{
    public class PotionItem : ConsumableItem
    {
        public override void Use()
        {
            // TODO
        }
        
        public override void Initialize(InventoryManager owner)
        {
            base.Initialize(owner);
            Type = ConsumableItemType.POTION;
        }
    }
}