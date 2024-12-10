using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI.Space
{
    public class UIInteraction : UIRootBase
    {
        protected override Context InitializeDataContext()
        {
            var context = UIManager.GetOrAddContext<InteractionContext>();
            return context;
        }
    }
}