using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    public class UIIntroScene : UIRootBase
    {        
        [Inject] UIManager _uiManager;
        
        protected override Context InitializeDataContext()
        {
            var context = new IntroSceneContext
            {
                UIManager = _uiManager
            };
            return context;
        }
    }
}
