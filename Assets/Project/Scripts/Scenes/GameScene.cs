using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class GameScene : BaseScene
    {
        [Inject] private SceneManagerEx _scene;
        
        protected override void Init()
        {
            base.Init();
            ESceneType = Define.eScene.Demo;
        }
        
        public override void Clear()
        {
        
        }
    }
}

