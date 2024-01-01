using GanShin.SceneManagement;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    public class UIIntroScene : UIRootBase
    {        
        [Inject] UIManager _uiManager;
        [Inject] SceneManagerEx _sceneManager;
        
        protected override Context InitializeDataContext()
        {
            var context = new IntroSceneContext
            {
                UIManager = _uiManager,
                SceneManager = _sceneManager,
            };
            return context;
        }
    }
}
