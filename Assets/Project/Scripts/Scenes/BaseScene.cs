using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public abstract class BaseScene : MonoBehaviour
    {
        void Awake()
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
        }

        public abstract void Clear();
    }
}