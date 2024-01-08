namespace GanShin.SceneManagement
{
    public class IntroScene : BaseScene
    {
        private SceneManagerEx Scene => ProjectManager.Instance.GetManager<SceneManagerEx>();

        protected override void Initialize()
        {
            base.Initialize();
            if (Scene.ESceneType != Define.eScene.INTRO)
                GanDebugger.LogWarning("Current logical scene is not IntroScene");
        }

        public override void Clear()
        {
        }
    }
}
