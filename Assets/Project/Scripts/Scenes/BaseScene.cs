using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
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

        /// <summary>
        /// 함수명은 Initialize이지만, ProjectManager상에서의 호출 순서는
        /// PostInitialize와 동일함
        /// </summary>
        protected virtual void Initialize()
        {
            ProjectManager.Instance.GetManager<CameraManager>()?.InitializeCamera();
            LoadSceneAssets().Forget();
        }

        protected abstract UniTask LoadSceneAssets();

        public abstract void Clear();
    }
}