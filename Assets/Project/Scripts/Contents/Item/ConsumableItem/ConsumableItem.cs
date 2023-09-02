namespace GanShin.Space.Content
{
    public abstract class ConsumableItem
    {
        public    ConsumableItemType Type  { get; protected set; }
        protected InventoryManager   Owner { get; private set; }

        public abstract void Use();

        public virtual void Initialize(InventoryManager owner)
        {
            Owner = owner;    
        }
    }
}