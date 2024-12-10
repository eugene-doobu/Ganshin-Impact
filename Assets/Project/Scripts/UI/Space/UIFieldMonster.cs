using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI.Space
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
