using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class IntroScene : BaseScene
    {
        [Inject] private SceneManagerEx _scene;

        protected override void Init()
        {
            base.Init();
            ESceneType = Define.eScene.INTRO;
        }

        public override void Clear()
        {
        }
    }
}
