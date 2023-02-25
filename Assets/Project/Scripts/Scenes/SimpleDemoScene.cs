using System;
using System.Collections;
using System.Collections.Generic;
using GanShin.SceneManagement;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class SimpleDemoScene : BaseScene
    {
        [Inject] 
        private PlayerManager _playerManager;
        
        protected override void Init()
        {
            base.Init();
            ESceneType = Define.eScene.Demo;
        }

        private void Start()
        {
            var riko = _playerManager.GetPlayer(Define.ePlayerAvatar.RIKO);
            if (riko == null) return;
            
            riko.transform.position = Vector3.zero;
            riko.gameObject.SetActive(true);
        }

        public override void Clear()
        {
        
        }
    }
}
