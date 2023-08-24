using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    public class UIMiniMap : UIRootBase
    {
        protected override Context InitializeDataContext() => new MinimapContext();
    }
}
