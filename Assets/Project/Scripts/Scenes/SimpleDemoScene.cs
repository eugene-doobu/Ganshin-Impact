using System;
using Cysharp.Threading.Tasks;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class SimpleDemoScene : BaseScene
    {
        [Inject] private PlayerManager _playerManager;

        [Inject] private SceneManagerEx _sceneManager;
        
        [SerializeField] private Define.ePlayerAvatar playerAvatar;

        [SerializeField] private Vector3 startPosition;
        
        protected override void Init()
        {
            base.Init();
            ESceneType = Define.eScene.SimpleDemo;
        }

        private void Start()
        {
            var player = _playerManager.SetCurrentPlayer(playerAvatar);
            player.transform.position = startPosition;
        }

        private void Update()
        {
            if (Input.GetKeyDown("]"))
                _sceneManager.LoadScene(ESceneType).Forget();
            
            if (Input.GetKeyDown("1"))
                _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.RIKO);
            
            if (Input.GetKeyDown("2"))
                _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.AI);
            
            if (Input.GetKeyDown("3"))
                _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.MUSCLE_CAT);
        }

        public override void Clear()
        {
        }
    }
}