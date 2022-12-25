using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.MapObject
{
    /// <summary>
    /// MapController에서 관리하는 오브젝트
    /// 기본적으로 Collider가 부착되어 있으며,
    /// 플레이어와의 거리에 따른 이벤트나 마우스 호버 이벤트 등이 존재한다.
    /// </summary>
    public abstract class MapObject : MonoBehaviour
    {
        // TODO: Apply Readonly
        [field:SerializeField] public long Id { get; set; }
        
        public void Awake()
        {
            Managers.MapObject.RegisterMapObject(this);
        }

        public virtual void OnUpdate()
        {
            
        }

        public void OnDestroy()
        {
            if (!Managers.InstanceExist) return;
            Managers.MapObject.RemoveMapObject(this);
        }
    }
}
