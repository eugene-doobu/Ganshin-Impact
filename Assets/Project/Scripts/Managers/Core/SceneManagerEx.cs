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

        public BaseScene CurrentScene => Object.FindObjectOfType<BaseScene>();

        public SceneManagerEx()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        public async UniTask LoadScene(Define.eScene type)
        {
            _ui.SetLoadingSceneUiActive(true);
            await SceneManager.LoadSceneAsync(GetSceneName(Define.eScene.LoadingScene)).ToUniTask();
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
            string name = System.Enum.GetName(typeof(Define.eScene), type);
            return name;
        }

        private void OnSceneUnLoaded(Scene scene)
        {
            if (CurrentScene != null)
                CurrentScene.Clear();
        }
    }
}