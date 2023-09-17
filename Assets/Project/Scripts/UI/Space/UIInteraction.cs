using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
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
