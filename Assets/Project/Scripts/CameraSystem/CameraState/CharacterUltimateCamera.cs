#nullable enable

using Cinemachine;
using GanShin.Resource;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.CameraSystem
{
    [UsedImplicitly]
    public class CharacterUltimateCamera : CameraBase
    {
        private CameraBodyTarget?           _cameraBodyTarget;
        private Cinemachine3rdPersonFollow? _body;
        private CinemachineComposer?        _aim;

        private void InitializeCameraBodyTarget()
        {
            var cameraBodyTargetObj = new GameObject("@CameraBodyTarget");
            Object.DontDestroyOnLoad(cameraBodyTargetObj);
            _cameraBodyTarget = cameraBodyTargetObj.GetOrAddComponent<CameraBodyTarget>();

            GanDebugger.CameraLog("CameraBody initialized");
        }
        
        private void InitializeVirtualCamera()
        {
            var virtualCameraPrefab = Resources.Load<GameObject>("Camera/PlayerUltimateVirtualCamera");
            if (virtualCameraPrefab == null)
            {
                GanDebugger.CameraLogError("Failed to load virtual camera prefab");
                return;
            }

            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            var virtualCameraObj = resourceManager?.Instantiate("PlayerUltimateVirtualCamera.prefab", isDontDestroy: true);
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

            Object.DontDestroyOnLoad(virtualCameraObj);

            GanDebugger.CameraLog("Virtual camera initialized");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (ReferenceEquals(_cameraBodyTarget, null)) return;
            if (ReferenceEquals(Target, null)) return;

            _cameraBodyTarget.SetRotation(Target.rotation);
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