using JetBrains.Annotations;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    [UsedImplicitly]
    public class IntroSceneContext : Context
    {
        public UIManager UIManager { get; set; }
        
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
            UIManager?.SetPopupOkCancel("종료하기", "게임을 종료하시겠습니까?",
                () =>
#if UNITY_EDITOR
                {
                    UnityEditor.EditorApplication.isPlaying = false;
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
