#nullable enable

using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.CameraSystem
{
    [UsedImplicitly]
    public class CharacterUltimateCamera : CameraBase
    {
        private CinemachineComposer?        _aim;
        private Cinemachine3rdPersonFollow? _body;
        private CameraBodyTarget?           _cameraBodyTarget;

        private void InitializeCameraBodyTarget()
        {
            _cameraBodyTarget = ProjectManager.Instance.GetManager<CameraManager>()?.CharacterBodyTarget;
            if (_cameraBodyTarget != null) return;
            
            GanDebugger.CameraLogError("Failed to get camera body target");
        }

        private void InitializeVirtualCamera()
        {
            var virtualCameraObj = Util.Instantiate("PlayerUltimateVirtualCamera.prefab");
            if (virtualCameraObj == null)
            {
                GanDebugger.CameraLogError("Failed to instantiate virtual camera prefab");
                return;
            }
            virtualCameraObj.name = "@PlayerUltimateVirtualCamera";

            VirtualCamera = virtualCameraObj.GetComponent<CinemachineVirtualCamera>();
            if (VirtualCamera == null)
            {
                GanDebugger.CameraLogError("Failed to get virtual camera component");
                return;
            }

            _body = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if (_body == null)
            {
                GanDebugger.CameraLogError("Failed to get body component");
                return;
            }

            _aim = VirtualCamera.GetCinemachineComponent<CinemachineComposer>();
            if (_aim == null)
            {
                GanDebugger.CameraLogError("Failed to get aim component");
                return;
            }

            GanDebugger.CameraLog("Virtual camera initialized");
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (ReferenceEquals(VirtualCamera, null))
                InitializeVirtualCamera();
        }

        public override void ChangeTarget(Transform? target)
        {
            base.ChangeTarget(target);

            if (ReferenceEquals(_cameraBodyTarget, null))
                InitializeCameraBodyTarget();

            _cameraBodyTarget!.SetTarget(target);

            if (ReferenceEquals(VirtualCamera, null))
                InitializeVirtualCamera();

            var cameraBodyTarget = _cameraBodyTarget.transform;
            VirtualCamera!.Follow = cameraBodyTarget;
            VirtualCamera.LookAt  = cameraBodyTarget;
        }
    }
}