#nullable enable

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GanShin.CameraSystem
{
    public enum eCameraState
    {
        DEFAULT,
        CHARACTER_CAMERA,
        CHARACTER_ULTIMATE_CAMERA
    }

    [UsedImplicitly]
    public class CameraManager : ManagerBase
    {
        [UsedImplicitly]
        public CameraManager()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        public override void Initialize()
        {
            CameraStateDictionaryInit();
            ChangeState(eCameraState.CHARACTER_CAMERA);
        }

        public override void Tick()
        {
            _currentCamera?.OnUpdate();
        }

        public override void LateTick()
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
            _cameraStates.Add(eCameraState.CHARACTER_CAMERA, new CharacterCamera());
            _cameraStates.Add(eCameraState.CHARACTER_ULTIMATE_CAMERA, new CharacterUltimateCamera());
            // TODO: InteractionCamera, CinematicCamera 등 추가
        }

#region CullingGroupProxy
        public CullingGroupProxy? GetOrAddCullingGroupProxy(eCullingGroupType cullingGroupType)
        {
            InitializeCamera();
            
            if (_ganCamera == null)
                return null;
            
            return _ganCamera.GetOrAddCullingGroupProxy(cullingGroupType);
        }
        
        public CullingGroupProxy SetCullingGroupProxy(GameObject gameObject, eCullingGroupType cullingGroupType)
        {
            var cullingGroups = gameObject.GetComponents<CullingGroupProxy>();

            CullingGroupProxy result;
            if (cullingGroups == null || cullingGroups.Length == 0)
            {
                result = gameObject.AddComponent<CullingGroupProxy>();
                result.SetCullingGroupType(cullingGroupType);
                return result;
            }

            foreach (var cullingGroup in cullingGroups)
                if (cullingGroup.CullingGroupType == cullingGroupType)
                    return cullingGroup;

            result = gameObject.AddComponent<CullingGroupProxy>();
            result.SetCullingGroupType(cullingGroupType);
            return result;
        }
#endregion CullingGroupProxy

#region Fields

        private readonly Dictionary<string, VirtualCameraJig> _virtualCameraDict = new();
        private readonly Dictionary<eCameraState, CameraBase> _cameraStates      = new();

        private readonly Dictionary<eCullingGroupType, float[]> _cullingGroupBoundingDistanceDict = new();

        private CameraBase? _currentCamera;

        private GanCamera? _ganCamera;
        private Camera?    _mainCamera;

#endregion Fields

#region Properties
        public Camera? MainCamera
        {
            set
            {
                _mainCamera = value;
                if (_mainCamera == null)
                {
                    _ganCamera = null;
                    return;
                }

                _ganCamera = _mainCamera.gameObject.GetOrAddComponent<GanCamera>();
            }
            get
            {
                if (ReferenceEquals(_mainCamera, null))
                {
                    _mainCamera = Camera.main;
                    if (_mainCamera == null) return null;
                    _ganCamera = _mainCamera.gameObject.GetOrAddComponent<GanCamera>();
                }

                return _mainCamera;
            }
        }

        public Transform? Target { get; set; }
#endregion Properties

#region VirtualCamera
        public void AddVirtualCamera(VirtualCameraJig jig)
        {
            _virtualCameraDict[jig.Name] = jig;
        }

        public void RemoveVirtualCamera(VirtualCameraJig jig)
        {
            if (!_virtualCameraDict.ContainsKey(jig.Name))
                GanDebugger.CameraLogError("RemoveVirtualCamera: jig not found");

            _virtualCameraDict.Remove(jig.Name);
        }
#endregion VirtualCamera

#region CullingMask
        public void SetCullingMask(int cullingMask)
        {
            var mainCamera = MainCamera;
            if (mainCamera == null) return;

            mainCamera.cullingMask = cullingMask;
        }

        public void OnCullingMaskLayer(int layerIndex)
        {
            OnCullingMaskLayer(MainCamera, layerIndex);
        }

        public void OffCullingMaskLayer(int layerIndex)
        {
            OffCullingMaskLayer(MainCamera, layerIndex);
        }

        public static void OnCullingMaskLayer(Camera? camera, int layerIndex)
        {
            if (camera == null) return;
            camera.cullingMask |= 1 << layerIndex;
        }

        public static void OffCullingMaskLayer(Camera? camera, int layerIndex)
        {
            if (camera == null) return;
            camera.cullingMask &= ~(1 << layerIndex);
        }

        public static void FlipCullingMaskLayer(Camera? camera, int layerIndex)
        {
            if (camera == null) return;
            camera.cullingMask ^= 1 << layerIndex;
        }

        public static bool GetCullingMaskLayer(Camera? camera, int layerIndex)
        {
            if (camera == null) return false;
            return (camera.cullingMask & (1 << layerIndex)) != 0;
        }
#endregion CullingMask

#region Utils
        public Ray GetRayFromCamera()
        {
            var currentEventSystem = EventSystem.current;
            if (currentEventSystem == null || currentEventSystem.IsPointerOverGameObject())
                return default;

            var clickPosition = Mouse.current.position.ReadValue();
            var screenWidth   = Screen.width;
            var screenHeight  = Screen.height;

            if (clickPosition.x < 0 || clickPosition.x >= screenWidth || clickPosition.y < 0 ||
                clickPosition.y >= screenHeight)
                return default;

            var mainCamera = MainCamera;
            if (mainCamera == null) return default;

            return mainCamera.ScreenPointToRay(clickPosition);
        }

        public bool IsFrontOfCamera(Vector3 worldPosition)
        {
            var mainCamera = MainCamera;
            if (mainCamera == null) return false;

            var tr = mainCamera.transform;
            return Vector3.Dot(tr.forward, (worldPosition - tr.position).normalized) > 0;
        }
        
        public void InitializeCamera()
        {
            MainCamera = Camera.main;
        }
#endregion Utils
    }
}