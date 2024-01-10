using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public abstract class BaseScene : MonoBehaviour
    {
        /// <summary>
        /// 씬의 모든 오브젝트가 초기화된 뒤 씬 객체를 초기화하기 위해
        /// Awake가 아닌 Start타이밍에 초기화를 진행함
        /// </summary>
        private void Start()
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