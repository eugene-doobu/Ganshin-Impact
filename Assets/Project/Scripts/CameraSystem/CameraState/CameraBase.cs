using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

#nullable enable

namespace GanShin.CameraSystem
{
    // TODO: 각 카메라 상태마다 VirtualCamera를 들고 있어야 한다.
    public abstract class CameraBase
    {
        [Inject] private CameraManager _camera = null!;
        
        protected Transform? Target { get; private set; }
        
        protected CinemachineVirtualCamera? VirtualCamera { get; set; }
        
        public virtual void OnEnable()
        {
            ChangeTarget(_camera.Target);

            if (!ReferenceEquals(VirtualCamera, null))
                VirtualCamera!.Priority = 100;
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnDisable()
        {

            if (!ReferenceEquals(VirtualCamera, null))
                VirtualCamera!.Priority = 0;
        }

        public virtual void ChangeTarget(Transform? target)
        {
            _camera.Target = target;
            Target         = target;
        }
    }
}