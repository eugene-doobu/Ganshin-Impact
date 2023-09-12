namespace GanShin.Space.Content
{
    public abstract class ConsumableItem : IUILoggable
    {
        public    ConsumableItemType Type  { get; protected set; }
        protected InventoryManager   Owner { get; private set; }
        
        public ConsumableItem(InventoryManager owner)
        {
            Owner = owner;
        }

        public virtual void Use()
        {
        }
    }
}