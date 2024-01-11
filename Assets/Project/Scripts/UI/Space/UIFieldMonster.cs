using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
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
