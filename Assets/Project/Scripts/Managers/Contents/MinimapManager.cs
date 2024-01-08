using GanShin.CameraSystem;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class MinimapManager : ManagerBase
    {
        // TODO: Addressables 로드로 변경
        private Camera MinimapCamera => ProjectManager.Instance.GetManager<CameraManager>()?.MinimapCamera;
        
        private PlayerManager PlayerManager => ProjectManager.Instance.GetManager<PlayerManager>();
        
        private RenderTexture _renderTexture;
        
        public Texture GetMinimapTexture()
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
            var playerTransform = PlayerManager.CurrentPlayerTransform;
            if (playerTransform == null)
                return;
            
            var tr       = MinimapCamera.transform;
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

            MinimapCamera.gameObject.SetActive(true);
            _renderTexture = new RenderTexture(350, 350, 24, RenderTextureFormat.ARGB32);
            MinimapCamera.targetTexture = _renderTexture;
        }
    }
}
