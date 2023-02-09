using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

#nullable enable

namespace GanShin.CameraSystem
{
    public enum eCameraState
    {
        DEFAULT,
        CHARACTER_CAMERA,
    }
    
    [UsedImplicitly]
    public class CameraManager : IInitializable, ITickable, ILateTickable
    {
        [Inject] private CharacterCamera? _characterCamera;
        
        private Dictionary<string, VirtualCameraJig> _virtualCameraDict = new();
        private Dictionary<eCameraState, CameraBase> _cameraStates      = new();
        
        private CameraBase? _currentCamera;

        private CinemachineBrain? _mainBrain;
        private Camera?           _mainCamera;
        public Camera? MainCamera
        {
            set
            {
                _mainCamera = value;
                if (_mainCamera == null)
                {
                    _mainBrain = null;
                    return;
                }
                _mainBrain = _mainCamera.gameObject.GetOrAddComponent<CinemachineBrain>();
            }
            get
            {
                if (ReferenceEquals(_mainCamera, null))
                {
                    _mainCamera = Camera.main;
                    if (_mainCamera == null) return null;
                    _mainBrain  = _mainCamera.gameObject.GetOrAddComponent<CinemachineBrain>();
                }
                return _mainCamera;
            }
        }
        
        [UsedImplicitly]
        public CameraManager()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }
        
        public void Initialize()
        {
            CameraStateDictionaryInit();
            ChangeState(eCameraState.CHARACTER_CAMERA);
        }
        
        public void Tick()
        {
            _currentCamera?.OnUpdate();
        }
        
        public void LateTick()
        {
            _currentCamera?.OnLateUpdate();
        }

        private void OnSceneUnLoaded(Scene scene)
        {
            MainCamera = null;
        }

        public void ChangeTarget(Transform transform)
        {
            _currentCamera?.ChangeTarget(transform);
        }
        
        public void ChangeState(eCameraState cameraState)
        {
            _currentCamera?.OnDisable();
            _currentCamera = _cameraStates[cameraState];
            _currentCamera.OnEnable();
        }
        
        private void CameraStateDictionaryInit()
        {
            _cameraStates.Add(eCameraState.CHARACTER_CAMERA, _characterCamera);
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
