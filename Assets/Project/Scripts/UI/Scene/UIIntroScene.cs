using GanShin.SceneManagement;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class UIIntroScene : UIRootBase
    {
        private readonly SceneManagerEx _sceneManager = ProjectManager.Instance.GetManager<SceneManagerEx>();
        private readonly UIManager      _uiManager    = ProjectManager.Instance.GetManager<UIManager>();

        protected override Context InitializeDataContext()
        {
            var context = new IntroSceneContext
            {
                UIManager    = _uiManager,
                SceneManager = _sceneManager
            };
            return context;
        }
    }
}