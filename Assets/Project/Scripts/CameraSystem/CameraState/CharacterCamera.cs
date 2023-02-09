using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GanShin.AssetManagement;
using GanShin.InputSystem;
using UnityEngine;
using Zenject;

#nullable enable

namespace GanShin.CameraSystem
{
    // Body : 3rd Person Follow
    // Aim : Composer
    public class CharacterCamera : CameraBase
    {
        #region Variables

        [Inject] private InputSystemManager? _input;
        [Inject] private ResourceManager?    _resource;
        
        private CameraBodyTarget?         _cameraBodyTarget;
        private CinemachineVirtualCamera? _virtualCamera;

        private Cinemachine3rdPersonFollow? _body;
        private CinemachineComposer?        _aim;

        private float _topClamp = 70f;
        private float _bottomClamp = -30f;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        
        private float _lookYawMagnitude = 0.2f;
        private float _lookPitchMagnitude = 0.15f;

        private float _targetZoom       = 2f;
        private float _zoomSmoothFactor = 4f;
        private float _zoomMagnitude    = 0.0015f;
        private float _zoomThreshHold   = 0.1f;
        private float _zoomMinValue     = 1f;
        private float _zoomMaxValue     = 4f;
        
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
            var virtualCameraPrefab = _resource!.Load<GameObject>("Camera/PlayerVirtualCamera");
            if (virtualCameraPrefab == null)
            {
                GanDebugger.CameraLogError("Failed to load virtual camera prefab");
                return;
            }
            
            var virtualCameraObj = Object.Instantiate(virtualCameraPrefab);
            virtualCameraObj.name = "@PlayerVirtualCamera";

            _virtualCamera = virtualCameraObj.GetComponent<CinemachineVirtualCamera>();
            if (_virtualCamera == null)
            {
                GanDebugger.CameraLogError("Failed to get virtual camera component");
                return;
            }
            
            _body = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if (_body == null)
            {
                GanDebugger.CameraLogError("Failed to get body component");
                return;
            }
            _targetZoom = _body.CameraDistance;

            _aim = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            if (_aim == null)
            {
                GanDebugger.CameraLogError("Failed to get aim component");
                return;
            }
            
            Object.DontDestroyOnLoad(virtualCameraObj);
            
            GanDebugger.CameraLog("Virtual camera initialized");
        }

        #endregion Initialization

        #region CameraBase

        public override void OnEnable()
        {
            base.OnEnable();
            AddInputEvent();
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
            
            if(ReferenceEquals(_cameraBodyTarget, null)) 
                InitializeCameraBodyTarget();
            
            _cameraBodyTarget!.SetTarget(target);
            
            if (ReferenceEquals(_virtualCamera, null)) 
                InitializeVirtualCamera();

            var cameraBodyTarget = _cameraBodyTarget.transform;
            _virtualCamera!.Follow = cameraBodyTarget;
            _virtualCamera.LookAt  = cameraBodyTarget;
        }
        
        #endregion CameraBase

        #region CameraProcess

        private void CameraRotation()
        {
            if(ReferenceEquals(_cameraBodyTarget, null)) 
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
                _body.CameraDistance = Mathf.Lerp(_body.CameraDistance, _targetZoom, Time.deltaTime * _zoomSmoothFactor);
        }

        #endregion CameraProcess
        
        #region Input

        private void AddInputEvent()
        {
            if (_input!.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
            {
                GanDebugger.CameraLogError("actionMap is null!");
                return;
            }

            actionMap.OnLook += OnLook;
            actionMap.OnZoom += OnZoom;
        }

        private void RemoveInputEvent()
        {
            if (_input == null) return;
            
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
                return;

            actionMap.OnLook -= OnLook;
            actionMap.OnZoom -= OnZoom;
        }
        
        private void OnLook(Vector2 value)
        {
            if (value == Vector2.zero)
                return;
            
            if(ReferenceEquals(_cameraBodyTarget, null)) 
                InitializeCameraBodyTarget();
            
            _cinemachineTargetYaw   += value.x * _lookYawMagnitude;
            _cinemachineTargetPitch += value.y * _lookPitchMagnitude;
        }

        private void OnZoom(float value)
        {
            value       *= _zoomMagnitude;
            _targetZoom =  Mathf.Clamp(_targetZoom + value, _zoomMinValue, _zoomMaxValue);
        }

        #endregion Input
    }
}
