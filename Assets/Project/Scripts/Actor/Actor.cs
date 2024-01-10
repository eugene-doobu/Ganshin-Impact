using System;
using Cysharp.Threading.Tasks;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.GanObject
{
    /// <summary>
    /// ActorManager에서 관리하는 오브젝트
    /// 기본적으로 Collider가 부착되어 있으며,
    /// 플레이어와의 거리에 따른 이벤트나 마우스 호버 이벤트 등이 존재한다.
    /// </summary>
    public abstract partial class Actor : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public long Id { get; set; }

        public bool IsMine { get; set; } = false;

        public float ObjectHeight { get; protected set; } = 0.5f;

        protected virtual void Awake()
        {
            WaitUntilInitialized().Forget();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void OnDestroy()
        {
            var actorManager = ProjectManager.Instance.GetManager<ActorManager>();
            actorManager?.RemoveActor(this);
        }

        public virtual void Tick()
        {
        }

        /// <summary>
        /// 함수명은 Initialize이지만, ProjectManager상에서의 호출 순서는
        /// PostInitialize와 동일함
        /// </summary>
        protected virtual void Initialize()
        {
            var actorManager = ProjectManager.Instance.GetManager<ActorManager>();
            if (actorManager == null)
            {
                GanDebugger.ActorLogError("Failed to get actor manager");
                return;
            }
            actorManager.RegisterActor(this);
        }

        private async UniTask WaitUntilInitialized()
        {
            await UniTask.WaitUntil(() => ProjectManager.Instance.IsInitialized);
            Initialize();
        }
    }
}