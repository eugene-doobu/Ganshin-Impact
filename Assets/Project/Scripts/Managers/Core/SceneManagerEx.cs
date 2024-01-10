using System;
using Cysharp.Threading.Tasks;
using GanShin.Resource;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GanShin.SceneManagement
{
    [UsedImplicitly]
    public class SceneManagerEx : ManagerBase
    {
        // TODO: Addressable로 변경
        //[Inject(Id = LoadingSettingInstaller.ChangeSceneDelayId)]
        private float _changeSceneDelay;

        [UsedImplicitly]
        public SceneManagerEx()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        private UIManager UIManager => ProjectManager.Instance.GetManager<UIManager>();

        public Define.eScene ESceneType { get; private set; } = Define.eScene.INTRO;

        private BaseScene _currentScene;
        
        public override void PostInitialize()
        {
            base.PostInitialize();
            _currentScene = Object.FindObjectOfType<BaseScene>();
        }

        public async UniTask LoadScene(Define.eScene type)
        {
            UIManager.SetLoadingSceneUiActive(true);
            ESceneType = type;
            ClearScene();
            await SceneManager.LoadSceneAsync(GetSceneName(Define.eScene.LOADING_SCENE)).ToUniTask();
            await UniTask.Delay(TimeSpan.FromMilliseconds(_changeSceneDelay));
            await SceneManager.LoadSceneAsync(GetSceneName(type))
                .ToUniTask(Progress.Create<float>(ApplyProgressToLoadingBar));
            await UniTask.NextFrame();
            _currentScene = Object.FindObjectOfType<BaseScene>();
            UIManager.SetLoadingSceneUiActive(false);
        }

        private void ApplyProgressToLoadingBar(float x)
        {
            var loadingScene = UIManager.GetGlobalUI(EGlobalUI.LOADING_SCENE) as UIRootLoadingScene;
            if (ReferenceEquals(loadingScene, null)) return;
            loadingScene.SetProgress(x);
        }

        // TODO: Addressable로 변경
        private string GetSceneName(Define.eScene type)
        {
            switch (type)
            {
                case Define.eScene.UNKNOWN:
                    return string.Empty;
                case Define.eScene.LOADING_SCENE:
                    return "Project/AddressableAssets/Scenes/LoadingScene";
                case Define.eScene.INTRO:
                    return "Project/AddressableAssets/Scenes/IntroScene";
                case Define.eScene.SIMPLE_DEMO:
                    return "Project/Scenes/SimpleDemo";
            }

            return string.Empty;
        }

        private void ClearScene()
        {
            if (_currentScene != null)
                _currentScene.Clear();
            _currentScene = null;

            UIManager.ClearContexts();

            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            resourceManager?.ReleaseAll();
        }

        private void OnSceneUnLoaded(Scene scene)
        {
        }
    }
}