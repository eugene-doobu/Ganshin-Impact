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

        protected override void Init()
        {
            base.Init();
            ESceneType = Define.eScene.SimpleDemo;
        }

        private void Start()
        {
            _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.AI);
        }

        private void Update()
        {
            if (Input.GetKeyDown("]"))
                _sceneManager.LoadScene(ESceneType).Forget();
        }

        public override void Clear()
        {
        }
    }
}