using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public abstract class BaseScene : MonoBehaviour
    {
        private void Awake()
        {
            WaitUntilInitialized().Forget();
        }

        private async UniTask WaitUntilInitialized()
        {
            await UniTask.WaitUntil(() => ProjectManager.Instance.IsInitialized);
            Initialize();
        }

        protected virtual void Initialize()
        {
            LoadSceneAssets().Forget();
        }

        protected abstract UniTask LoadSceneAssets();

        public abstract void Clear();
    }
}