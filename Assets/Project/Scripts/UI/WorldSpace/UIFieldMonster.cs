using GanShin.UI.ViewModels.WorldSpace;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI.WorldSpace
{
    public class UIFieldMonster : UIRootBase
    {
        protected override Context InitializeDataContext()
        {
            var context = UIManager.GetOrAddContext<FieldMonsterManagerContext>();
            return context;
        }
    }
}
