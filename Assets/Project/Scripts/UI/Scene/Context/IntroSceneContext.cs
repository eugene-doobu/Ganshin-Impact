using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    [UsedImplicitly]
    public class IntroSceneContext : Context
    {
        [Inject] private UIManager _uiManager;
        
        [UsedImplicitly]
        public void OnClickStart()
        {
            GanDebugger.Log(GetType().Name,"OnClickStart");
        }
        
        [UsedImplicitly]
        public void OnClickNewGame()
        {
            GanDebugger.Log(GetType().Name,"OnClickNewGame");
        }
        
        [UsedImplicitly]
        public void OnClickSetting()
        {
            GanDebugger.Log(GetType().Name,"OnClickSetting");
        }

        [UsedImplicitly]
        public void OnClickExit()
        {
            _uiManager.SetPopupOkCancel("종료하기", "게임을 종료하시겠습니까?",
                () =>
#if UNITY_EDITOR
                {
                    Application.Quit();
                }
#else
            {
                    Application.Quit();
            }
#endif
            );
        }
    }
}
