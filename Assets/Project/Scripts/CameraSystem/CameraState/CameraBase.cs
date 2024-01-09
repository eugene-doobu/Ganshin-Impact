#nullable enable

using Cinemachine;
using UnityEngine;

namespace GanShin.CameraSystem
{
    // TODO: 각 카메라 상태마다 VirtualCamera를 들고 있어야 한다.
    public abstract class CameraBase
    {
        private CameraManager? Camera => ProjectManager.Instance.GetManager<CameraManager>();

        protected Transform? Target { get; private set; }

        protected CinemachineVirtualCamera? VirtualCamera { get; set; }

        public virtual void OnEnable()
        {
            if (Camera == null)
                return;

            ChangeTarget(Camera.Target);

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
            if (Camera != null) Camera.Target = target;
            Target = target;
        }
    }
}