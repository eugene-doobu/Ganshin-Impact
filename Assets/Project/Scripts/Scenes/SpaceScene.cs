#nullable enable

using GanShin.CameraSystem;
using GanShin.Resource;
using GanShin.UI;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public abstract class SpaceScene : BaseScene
    {
        private GameObject? _canvasRoot;

        protected override void Initialize()
        {
            base.Initialize();
            var cameraManager = ProjectManager.Instance.GetManager<CameraManager>();
            cameraManager?.GetOrAddCullingGroupProxy(eCullingGroupType.OBJECT_HUD);
            
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (resourceManager == null)
            {
                GanDebugger.LogError("Failed to get resource manager");
                return;
            }
            _canvasRoot = resourceManager.Instantiate("Canvas_SpaceScene.prefab");
            
            var uiManager = ProjectManager.Instance.GetManager<UIManager>();
            uiManager?.EnableControlObjectUI();
        }

        public override void Clear()
        {
            var uiManager = ProjectManager.Instance.GetManager<UIManager>();
            uiManager?.DisableControlObjectUI();
            
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (_canvasRoot != null && resourceManager != null)
                resourceManager.Destroy(_canvasRoot);
        }
    }
}