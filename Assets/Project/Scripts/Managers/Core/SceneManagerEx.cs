using System;
using Cysharp.Threading.Tasks;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace GanShin.SceneManagement
{
    [UsedImplicitly]
    public class SceneManagerEx
    {
        [Inject] private UIManager          _ui;
        [Inject] private UIRootLoadingScene _loadingScene;

        [Inject(Id = LoadingSettingInstaller.ChangeSceneDelayId)]
        private float _changeSceneDelay;

        public Define.eScene ESceneType { get; private set; } = Define.eScene.INTRO;

        public BaseScene CurrentScene => Object.FindObjectOfType<BaseScene>();

        public SceneManagerEx()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        public async UniTask LoadScene(Define.eScene type)
        {
            _ui.SetLoadingSceneUiActive(true);
            ESceneType = type;
            await SceneManager.LoadSceneAsync(GetSceneName(Define.eScene.LOADING_SCENE)).ToUniTask();
            await UniTask.Delay(TimeSpan.FromMilliseconds(_changeSceneDelay));
            await SceneManager.LoadSceneAsync(GetSceneName(type))
                .ToUniTask(Progress.Create<float>(ApplyProgressToLoadingBar));
            await UniTask.NextFrame();
            _ui.SetLoadingSceneUiActive(false);
        }

        private void ApplyProgressToLoadingBar(float x)
        {
            if (ReferenceEquals(_loadingScene, null)) return;
            _loadingScene.SetProgress(x);
        }

        string GetSceneName(Define.eScene type)
        {
            switch (type)
            {
                case Define.eScene.UNKNOWN:
                    return string.Empty;
                case Define.eScene.LOADING_SCENE:
                    return "LoadingScene";
                case Define.eScene.INTRO:
                    return "IntroScene";
                case Define.eScene.SIMPLE_DEMO:
                    return "SimpleDemo";
            }

            return string.Empty;
        }

        private void OnSceneUnLoaded(Scene scene)
        {
            if (CurrentScene != null)
                CurrentScene.Clear();
            
            _ui.ClearContexts();
        }
    }
}