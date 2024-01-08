using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            var player = _playerManager.SetCurrentPlayer(playerAvatar);
            if (player != null)
                player.transform.position = startPosition;
        }

        public override void Clear()
        {
        }
    }
}
