using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    public abstract class CameraBase
    {
        protected Transform? Target { get; private set; }
        
        public virtual void OnEnable()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnLateUpdate()
        {
            
        }

        public virtual void OnDisable()
        {
            
        }

        public virtual void ChangeTarget(Transform? target)
        {
            Target = target;
        }
    }
}
