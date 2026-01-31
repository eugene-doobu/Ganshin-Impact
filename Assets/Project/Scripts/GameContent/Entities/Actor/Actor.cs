#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
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

        public bool IsMine { get; set; }

        public float ObjectHeight { get; protected set; } = 0.5f;

        /// <summary>
        /// Actor의 활성화 상태를 추적합니다.
        /// OnSpawn 시 true, OnDespawn 시 false가 됩니다.
        /// </summary>
        private bool _isActorActive;
        public bool IsActorActive => _isActorActive;

        /// <summary>
        /// ActorManager에 등록되었는지 여부를 추적합니다.
        /// 중복 등록/해제를 방지합니다.
        /// </summary>
        private bool _isRegistered;

        /// <summary>
        /// 풀에서 생성된 오브젝트인지 여부
        /// PoolManager에서 생성 시 true로 설정됩니다.
        /// </summary>
        public bool Pooling { get; set; }

#region Mono
        protected virtual void Awake()
        {
            WaitUntilInitialized().Forget();
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnEnable()
        {
            if (!_isActorActive) return;

            RegisterToActorManager();
        }

        protected virtual void OnDisable()
        {
            if (!_isActorActive) return;

            UnregisterFromActorManager();
        }

        protected virtual void OnDestroy()
        {
            if (!_isActorActive) return;

            UnregisterFromActorManager();
        }
#endregion Mono

#region Actor
        /// <summary>
        /// 함수명은 Initialize이지만, ProjectManager상에서의 호출 순서는
        /// PostInitialize와 동일함
        /// </summary>
        protected virtual void Initialize()
        {
            _isActorActive = true;
            RegisterToActorManager();
        }

        /// <summary>
        /// Tick 전에 호출되는 메서드
        /// </summary>
        public virtual void PreTick()
        {
        }

        public virtual void Tick()
        {
        }

        /// <summary>
        /// Tick 후에 호출되는 메서드
        /// </summary>
        public virtual void LateTick()
        {
        }

        private async UniTask WaitUntilInitialized()
        {
            await UniTask.WaitUntil(() => ProjectManager.Instance.IsInitialized);
            Initialize();
            OnManagersReady();
        }

        /// <summary>
        /// 매니저 초기화 완료 후 호출됩니다.
        /// Initialize() 이후에 호출되며, 매니저에 의존하는 로직을 여기서 처리합니다.
        /// </summary>
        protected virtual void OnManagersReady()
        {
        }

        /// <summary>
        /// 풀에서 오브젝트를 꺼낼 때 호출됩니다.
        /// 활성화 시 필요한 초기화 로직을 여기서 처리합니다.
        /// </summary>
        public virtual void OnSpawn()
        {
            _isActorActive = true;
            RegisterToActorManager();
        }

        /// <summary>
        /// 풀로 오브젝트를 반환할 때 호출됩니다.
        /// 비활성화 시 필요한 정리 로직을 여기서 처리합니다.
        /// </summary>
        public virtual void OnDespawn()
        {
            _isActorActive = false;
            UnregisterFromActorManager();
        }

        /// <summary>
        /// ActorManager에 등록합니다.
        /// 이미 등록된 경우 중복 등록을 방지합니다.
        /// </summary>
        private void RegisterToActorManager()
        {
            if (_isRegistered) return;

            var actorManager = ProjectManager.Instance.GetManager<ActorManager>();
            if (actorManager == null)
            {
                GanDebugger.ActorLogError("Failed to get actor manager");
                return;
            }

            actorManager.RegisterActor(this);
            _isRegistered = true;
        }

        /// <summary>
        /// ActorManager에서 등록 해제합니다.
        /// 등록되지 않은 경우 중복 해제를 방지합니다.
        /// </summary>
        private void UnregisterFromActorManager()
        {
            if (!_isRegistered) return;

            var actorManager = ProjectManager.Instance.GetManager<ActorManager>();
            actorManager?.RemoveActor(this);
            _isRegistered = false;
        }

        public virtual void OnRegister()
        {
            var cameraManager = ProjectManager.Instance.GetManager<CameraManager>();
            var cullingGroup = cameraManager?.GetOrAddCullingGroupProxy(eCullingGroupType.OBJECT_HUD);
            if (cullingGroup == null) return;
            cullingGroup.Add(this);
            OnHudStateChanged += OnHudCullingGroupStateChanged;
        }

        public virtual void OnUnregister()
        {
            var cameraManager = ProjectManager.Instance.GetManager<CameraManager>();
            var cullingGroup  = cameraManager?.GetOrAddCullingGroupProxy(eCullingGroupType.OBJECT_HUD);
            if (cullingGroup == null) return;
            cullingGroup.Remove(this);
            OnHudStateChanged -= OnHudCullingGroupStateChanged;
        }
#endregion Actor
    }
}