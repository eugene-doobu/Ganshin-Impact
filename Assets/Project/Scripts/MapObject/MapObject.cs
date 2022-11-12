using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.MapObject
{
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
