using GanShin.SceneManagement;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    public class UIIntroScene : UIRootBase
    {
        private UIManager      _uiManager    = ProjectManager.Instance.GetManager<UIManager>();
        private SceneManagerEx _sceneManager = ProjectManager.Instance.GetManager<SceneManagerEx>();
        
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
