using GanShin.Utils;
using UnityEngine;
using Zenject;

namespace GanShin.GanObject
{
    /// <summary>
    /// MapController에서 관리하는 오브젝트
    /// 기본적으로 Collider가 부착되어 있으며,
    /// 플레이어와의 거리에 따른 이벤트나 마우스 호버 이벤트 등이 존재한다.
    /// </summary>
    public abstract partial class MapObject : MonoBehaviour
    {
        [Inject] protected MapObjectManager _mapObjectManager;

        [field: SerializeField, ReadOnly] public long Id { get; set; }

        public bool IsMine { get; set; } = false;

        public float ObjectHeight { get; protected set; } = 0.5f;

        public void Awake()
        {
            _mapObjectManager.RegisterMapObject(this);
        }

        public virtual void OnUpdate()
        {
        }

        public void OnDestroy()
        {
            if (_mapObjectManager == null) return;
            _mapObjectManager.RemoveMapObject(this);
        }
    }
}