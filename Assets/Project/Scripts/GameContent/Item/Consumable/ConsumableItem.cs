namespace GanShin.Item
{
    public abstract class ConsumableItem : IUILoggable
    {
        public ConsumableItem(InventoryManager owner)
        {
            Owner = owner;
        }

        public    ConsumableItemType Type  { get; protected set; }
        protected InventoryManager   Owner { get; private set; }

        public virtual void Use()
        {
        }
    }
}