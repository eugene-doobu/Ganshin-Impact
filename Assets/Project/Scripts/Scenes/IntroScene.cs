using Cysharp.Threading.Tasks;
using GanShin.Resource;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public class IntroScene : BaseScene
    {
        protected override void Initialize()
        {
            base.Initialize();
            var scene = ProjectManager.Instance.GetManager<SceneManagerEx>();
            if (scene == null)
            {
                GanDebugger.LogError("Failed to get scene manager");
                return;
            }
            
            if (scene.ESceneType != Define.eScene.INTRO)
                GanDebugger.LogWarning("Current logical scene is not IntroScene");
        }

        protected override async UniTask LoadSceneAssets()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (resourceManager == null)
            {
                GanDebugger.LogError("Failed to get resource manager");
                return;
            }
            
            await resourceManager.LoadAllAsync<Object>("Intro");
            
            resourceManager.Instantiate("Canvas_IntroScene.prefab");
        }

        public override void Clear()
        {
        }
    }
}
