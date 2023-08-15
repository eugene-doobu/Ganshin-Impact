using System.ComponentModel;

namespace GanShin.UI
{
    public class UIRootDimmed : GlobalUIRootBase
    {
        protected override INotifyPropertyChanged InitializeDataContext() => null;

        public override void InitializeContextData() { }

        public override void ClearContextData() { }
    }
}
