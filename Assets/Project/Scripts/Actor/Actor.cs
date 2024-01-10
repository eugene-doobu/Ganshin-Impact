using GanShin.Utils;
using UnityEngine;

namespace GanShin.GanObject
{
    /// <summary>
    ///     MapController에서 관리하는 오브젝트
    ///     기본적으로 Collider가 부착되어 있으며,
    ///     플레이어와의 거리에 따른 이벤트나 마우스 호버 이벤트 등이 존재한다.
    /// </summary>
    public abstract partial class Actor : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public long Id { get; set; }

        protected ActorManager ActorManager => ProjectManager.Instance.GetManager<ActorManager>();

        public bool IsMine { get; set; } = false;

        public float ObjectHeight { get; protected set; } = 0.5f;

        public void Awake()
        {
            ActorManager.RegisterActor(this);
        }

        public virtual void OnUpdate()
        {
        }

        public void OnDestroy()
        {
            if (ActorManager == null) return;
            ActorManager.RemoveActor(this);
        }
    }
}