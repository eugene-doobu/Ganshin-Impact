namespace GanShin.UI
{
    public abstract class GlobalUIRootBase : UIRootBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            InitializeContextData();
        }

        public void Disable()
        {
            ClearContextData();
            gameObject.SetActive(false);
        }

        public abstract void InitializeContextData();

        public abstract void ClearContextData();
    }
}