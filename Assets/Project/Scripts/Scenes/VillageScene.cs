using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GanShin.Resource;
using GanShin.UI;
using UnityEngine;

namespace GanShin.SceneManagement
{
    public class VillageScene : SpaceScene
    {
        private PlayerManager _playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
        private SceneManagerEx _sceneManager = ProjectManager.Instance.GetManager<SceneManagerEx>();
        private UIManager _uiManager = ProjectManager.Instance.GetManager<UIManager>();
        
        [SerializeField] private Define.ePlayerAvatar playerAvatar;

        [SerializeField] private Vector3 startPosition;
        
        protected override void Initialize()
        {
            base.Initialize();
            if (_sceneManager.ESceneType != Define.eScene.VILLAGE)
                GanDebugger.LogWarning("Current logical scene is not VillageScene");
            
            var player = _playerManager.SetCurrentPlayer(playerAvatar);
            if (player != null)
                player.transform.position = startPosition;
        }

        protected override async UniTask LoadSceneAssets()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (resourceManager == null)
            {
                GanDebugger.LogError("Failed to get resource manager");
                return;
            }
            
            await resourceManager.LoadAllAsync<Object>("Village");
        }

        public override void Clear()
        {
        }
    }
}
