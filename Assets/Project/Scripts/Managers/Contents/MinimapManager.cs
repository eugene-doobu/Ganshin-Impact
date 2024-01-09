#nullable enable

using GanShin.Resource;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class MinimapManager : ManagerBase
    {
        [UsedImplicitly] public MinimapManager() { }
        
        private Camera? _miniMapCamera = null;
        private RenderTexture? _renderTexture;
        
        public Texture? GetMinimapTexture()
        {
            if (_renderTexture == null)
                SetCameraRenderTarget();
            return _renderTexture;
        }
        
        public override void Initialize()
        {
            SetCameraRenderTarget();
        }

        public override void LateTick()
        {
            var playerManager   = ProjectManager.Instance.GetManager<PlayerManager>();
            var playerTransform = playerManager?.CurrentPlayerTransform;
            if (playerTransform == null || _miniMapCamera == null)
                return;
            
            var tr       = _miniMapCamera.transform;
            var position = tr.position;
            var height   = position.y;
            position    = playerTransform.position;
            position    = new Vector3(position.x, height, position.z);
            tr.position = position;
        }

        private void SetCameraRenderTarget()
        {
            if (_renderTexture != null)
                return;
            
            var resourceManager  = ProjectManager.Instance.GetManager<ResourceManager>();
            var minimapCamera = resourceManager?.Instantiate("MinimapCamera.prefab", isDontDestroy: true);
            if (minimapCamera == null)
            {
                GanDebugger.CameraLogError("Failed to instantiate virtual camera prefab");
                return;
            }

            _miniMapCamera = minimapCamera.GetComponent<Camera>();
            _miniMapCamera.gameObject.SetActive(true);
            _renderTexture               = new RenderTexture(350, 350, 24, RenderTextureFormat.ARGB32);
            _miniMapCamera.targetTexture = _renderTexture;
        }
    }
}
