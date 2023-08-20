using System.Collections;
using System.Collections.Generic;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class VillageScene : SpaceScene
    {
        
        [Inject] private PlayerManager _playerManager;

        [Inject] private SceneManagerEx _sceneManager;
        
        [Inject] private UIManager _uiManager;
        
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
