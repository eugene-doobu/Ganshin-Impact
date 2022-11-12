using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GanShin.InputSystem;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    public class CharacterCamera : CameraBase
    {
        #region Variables

        private readonly InputSystemManager _input;
        
        private CameraBodyTarget?         _cameraBodyTarget;
        private CinemachineVirtualCamera? _virtualCamera;

        private float _topClamp = 70f;
        private float _bottomClamp = -30f;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        
        private float _lookYawMagnitude = 0.2f;
        private float _lookPitchMagnitude = 0.15f;
        
        #endregion Variables

        #region Initialization

        public CharacterCamera()
        {
            _input = Managers.Input;
        }

        private void InitializeCameraBodyTarget()
        {  
            var cameraBodyTargetObj = new GameObject("@CameraBodyTarget");
            Object.DontDestroyOnLoad(cameraBodyTargetObj);
            _cameraBodyTarget = cameraBodyTargetObj.GetOrAddComponent<CameraBodyTarget>();
            
            GanDebugger.CameraLog("CameraBody initialized");
        }

        private void InitializeVirtualCamera()
        {
            var virtualCameraPrefab = Managers.Resource.Load<GameObject>("Camera/PlayerVirtualCamera");
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

        #region RotateCamera

        private void CameraRotation()
        {
            if(ReferenceEquals(_cameraBodyTarget, null)) 
                InitializeCameraBodyTarget();
            
            _cinemachineTargetYaw   = MathUtils.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = MathUtils.ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
            
            var targetRotate = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
            _cameraBodyTarget!.SetRotation(targetRotate);
        }

        #endregion RotateCamera
        
        #region Input

        private void AddInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
            {
                GanDebugger.CameraLogError("actionMap is null!");
                return;
            }

            actionMap.OnLook += OnLook;
        }

        private void RemoveInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
                return;

            actionMap.OnLook -= OnLook;
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

        #endregion Input
    }
}
