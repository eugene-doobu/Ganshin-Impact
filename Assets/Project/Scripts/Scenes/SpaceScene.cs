#nullable enable

using Cysharp.Threading.Tasks;
using GanShin.Resource;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public abstract class SpaceScene : BaseScene
    {
        private GameObject? _canvasRoot;
        
        protected override async UniTask LoadSceneAssets()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (resourceManager == null)
            {
                GanDebugger.LogError("Failed to get resource manager");
                return;
            }
            
            _canvasRoot = resourceManager.Instantiate("Canvas_SpaceScene.prefab");

            await UniTask.CompletedTask;
        }
        
        public override void Clear()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (_canvasRoot != null && resourceManager != null)
                resourceManager.Destroy(_canvasRoot);
        }
    }
}
