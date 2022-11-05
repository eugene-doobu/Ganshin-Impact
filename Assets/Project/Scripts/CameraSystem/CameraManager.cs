using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.CameraSystem
{
    public enum eCameraState
    {
        AVATAR_CAMERA,
    }
    
    public class CameraManager
    {
        private Dictionary<eCameraState, CameraBase> _cameraStates = new();
        private CameraBase _currentCamera;

        public void Init()
        {
            CameraStateDictionaryInit();
            ChangeCamera(eCameraState.AVATAR_CAMERA);
        }

        public void OnUpdate()
        {
            _currentCamera?.OnUpdate();
        }

        public void OnLateUpdate()
        {
            _currentCamera?.OnLateUpdate();
        }
        
        public void ChangeCamera(eCameraState cameraState)
        {
            _currentCamera?.OnDisable();
            _currentCamera = _cameraStates[cameraState];
            _currentCamera.OnEnable();
        }
        
        private void CameraStateDictionaryInit()
        {
            _cameraStates.Add(eCameraState.AVATAR_CAMERA, new AvatarCamera());
            // TODO: InteractionCamera, CinematicCamera 등 추가
        }
    }
}
