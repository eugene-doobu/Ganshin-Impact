#nullable enable

using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class MinimapManager : ManagerBase
    {
        private Camera?        _miniMapCamera;
        private RenderTexture? _renderTexture;

        [UsedImplicitly]
        public MinimapManager()
        {
        }

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
            if (_miniMapCamera == null || playerTransform == null)
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

            var minimapCamera = Util.Instantiate("MinimapCamera.prefab");
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