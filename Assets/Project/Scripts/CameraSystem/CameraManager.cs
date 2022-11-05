using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.CameraSystem
{
    public enum eCameraState
    {
        DEFAULT,
        CHARACTER_CAMERA,
    }
    
    public class CameraManager
    {
        private Dictionary<string, VirtualCameraJig> _virtualCameraDict = new();
        private Dictionary<eCameraState, CameraBase> _cameraStates      = new();
        
        private CameraBase _currentCamera;

        public void Init()
        {
            CameraStateDictionaryInit();
            ChangeState(eCameraState.CHARACTER_CAMERA);
        }

        public void OnUpdate()
        {
            _currentCamera?.OnUpdate();
        }

        public void OnLateUpdate()
        {
            _currentCamera?.OnLateUpdate();
        }
        
        public void ChangeState(eCameraState cameraState)
        {
            _currentCamera?.OnDisable();
            _currentCamera = _cameraStates[cameraState];
            _currentCamera.OnEnable();
        }
        
        private void CameraStateDictionaryInit()
        {
            _cameraStates.Add(eCameraState.CHARACTER_CAMERA, new CharacterCamera());
            // TODO: InteractionCamera, CinematicCamera 등 추가
        }

        #region VirtualCamera

        public void AddVirtualCamera(VirtualCameraJig jig)
        {
            _virtualCameraDict[jig.Name] = jig;
        }
        
        public void RemoveVirtualCamera(VirtualCameraJig jig)
        {
            if (!_virtualCameraDict.ContainsKey(jig.Name))
            {
                GanDebugger.CameraLogError("RemoveVirtualCamera: jig not found");
            }
            _virtualCameraDict.Remove(jig.Name);
        }

        #endregion VirtualCamera
    }
}
