using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class UIRootDimmed : GlobalUIRootBase
    {
        protected override Context InitializeDataContext()
        {
            return null;
        }

        public override void InitializeContextData()
        {
        }

        public override void ClearContextData()
        {
        }
    }
}