using JetBrains.Annotations;

namespace GanShin
{
    public abstract class ManagerBase
    {
        [UsedImplicitly]
        protected ManagerBase(){ }
        
        public virtual void Initialize()
        {
        
        }

        public virtual void Tick()
        {
        
        }
        
        public virtual void LateTick()
        {
        
        }
    }
}
