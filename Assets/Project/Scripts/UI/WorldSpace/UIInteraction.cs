using GanShin.UI.ViewModels.WorldSpace;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI.WorldSpace
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