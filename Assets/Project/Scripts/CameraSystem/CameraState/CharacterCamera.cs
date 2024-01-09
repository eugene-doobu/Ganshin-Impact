#nullable enable

using Cinemachine;
using GanShin.InputSystem;
using GanShin.Resource;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.CameraSystem
{
    // Body : 3rd Person Follow
    // Aim : Composer
    [UsedImplicitly]
    public class CharacterCamera : CameraBase
    {
#region TableDatas

        private float _topClamp;
        private float _bottomClamp;
        private float _lookYawMagnitude;
        private float _lookPitchMagnitude;
        private float _targetZoom;
        private float _zoomSmoothFactor;
        private float _zoomMagnitude;
        private float _zoomThreshHold;
        private float _zoomMinValue;
        private float _zoomMaxValue;

#endregion TableDatas

#region Variables

        private InputSystemManager? Input => ProjectManager.Instance.GetManager<InputSystemManager>();

        private CameraBodyTarget?           _cameraBodyTarget;
        private Cinemachine3rdPersonFollow? _body;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

#endregion Variables

#region Initialization

        private void InitializeCameraBodyTarget()
        {
            var cameraBodyTargetObj = new GameObject("@CameraBodyTarget");
            Object.DontDestroyOnLoad(cameraBodyTargetObj);
            _cameraBodyTarget = cameraBodyTargetObj.GetOrAddComponent<CameraBodyTarget>();

            GanDebugger.CameraLog("CameraBody initialized");
        }

        private void InitializeVirtualCamera()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();

            var virtualCameraPrefab = resourceManager?.Load<GameObject>("PlayerVirtualCamera.prefab");
            if (virtualCameraPrefab == null)
            {
                GanDebugger.CameraLogError("Failed to load virtual camera prefab");
                return;
            }

            var virtualCameraObj = resourceManager?.Instantiate("PlayerVirtualCamera.prefab");
            if (virtualCameraObj == null)
            {
                GanDebugger.CameraLogError("Failed to instantiate virtual camera prefab");
                return;
            }

            virtualCameraObj.name = "@PlayerVirtualCamera";

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

            _targetZoom = _body.CameraDistance;

            Object.DontDestroyOnLoad(virtualCameraObj);

            GanDebugger.CameraLog("Virtual camera initialized");
        }

        private void InitializeCameraSetting()
        {
            var data = Util.LoadAsset<CharacterCameraSettingInstaller>("CharacterCameraSetting.asset");
            if (data == null)
            {
                GanDebugger.CameraLogError("Failed to load CharacterCameraSetting.asset");
                return;
            }

            _topClamp           = data.topClamp;
            _bottomClamp        = data.bottomClamp;
            _lookYawMagnitude   = data.lookYawMagnitude;
            _lookPitchMagnitude = data.lookPitchMagnitude;
            _targetZoom         = data.targetZoom;
            _zoomSmoothFactor   = data.zoomSmoothFactor;
            _zoomMagnitude      = data.zoomMagnitude;
            _zoomThreshHold     = data.zoomThreshHold;
            _zoomMinValue       = data.zoomMinValue;
            _zoomMaxValue       = data.zoomMaxValue;
        }

#endregion Initialization

#region CameraBase

        public CharacterCamera()
        {
            InitializeCameraSetting();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            AddInputEvent();

            if (ReferenceEquals(VirtualCamera, null))
                InitializeVirtualCamera();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            CameraRotation();
            CameraZoom();
        }

        public override void OnDisable()
        {
            RemoveInputEvent();
            base.OnDisable();
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

#endregion CameraBase

#region CameraProcess

        private void CameraRotation()
        {
            if (ReferenceEquals(_cameraBodyTarget, null))
                InitializeCameraBodyTarget();

            _cinemachineTargetYaw   = MathUtils.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = MathUtils.ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

            var targetRotate = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
            _cameraBodyTarget!.SetRotation(targetRotate);
        }

        private void CameraZoom()
        {
            if (_body == null) return;
            if (_zoomThreshHold < Mathf.Abs(_body.CameraDistance - _targetZoom))
                _body.CameraDistance =
                    Mathf.Lerp(_body.CameraDistance, _targetZoom, Time.deltaTime * _zoomSmoothFactor);
        }

#endregion CameraProcess

#region Input

        private void AddInputEvent()
        {
            if (Input!.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
            {
                GanDebugger.CameraLogError("actionMap is null!");
                return;
            }

            actionMap.OnLook += OnLook;
            actionMap.OnZoom += OnZoom;
        }

        private void RemoveInputEvent()
        {
            if (Input == null) return;

            if (Input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
                return;

            actionMap.OnLook -= OnLook;
            actionMap.OnZoom -= OnZoom;
        }

        private void OnLook(Vector2 value)
        {
            if (value == Vector2.zero)
                return;

            if (ReferenceEquals(_cameraBodyTarget, null))
                InitializeCameraBodyTarget();

            _cinemachineTargetYaw   += value.x * _lookYawMagnitude;
            _cinemachineTargetPitch += value.y * _lookPitchMagnitude;
        }

        private void OnZoom(float value)
        {
            value       *= _zoomMagnitude;
            _targetZoom =  Mathf.Clamp(_targetZoom - value, _zoomMinValue, _zoomMaxValue);
        }

#endregion Input
    }
}