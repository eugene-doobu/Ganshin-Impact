using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class MinimapManager : IInitializable, ILateTickable
    {
        public const string MinimapCameraId = "MinimapManager.MinimapCamera";
        
        [Inject(Id = MinimapCameraId)]
        private Camera _minimapCamera = null!;
        
        [Inject]
        private PlayerManager _playerManager = null!;
        
        private RenderTexture _renderTexture;
        
        public Texture GetMinimapTexture()
        {
            if (_renderTexture == null)
                SetCameraRenderTarget();
            return _renderTexture;
        }
        
        public void Initialize()
        {
            SetCameraRenderTarget();
        }

        private void SetCameraRenderTarget()
        {
            if (_renderTexture != null)
                return;

            _minimapCamera.gameObject.SetActive(true);
            _renderTexture = new RenderTexture(350, 350, 24, RenderTextureFormat.ARGB32);
            _minimapCamera.targetTexture = _renderTexture;
        }

        public void LateTick()
        {
            var tr       = _minimapCamera.transform;
            var position = tr.position;
            var height   = position.y;
            position    = _playerManager.CurrentPlayerTransform!.position;
            position    = new Vector3(position.x, height, position.z);
            tr.position = position;
        }
    }
}
