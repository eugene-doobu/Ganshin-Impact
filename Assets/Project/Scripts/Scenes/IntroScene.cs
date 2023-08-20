using Zenject;

namespace GanShin.SceneManagement
{
    public class IntroScene : BaseScene
    {
        [Inject] private SceneManagerEx _scene;

        protected override void Init()
        {
            base.Init();
            if (_scene.ESceneType != Define.eScene.INTRO)
                GanDebugger.LogWarning("Current logical scene is not IntroScene");
        }

        public override void Clear()
        {
        }
    }
}
