using GanShin.SceneManagement;
using JetBrains.Annotations;
using UnityEditor;

namespace GanShin.UI
{
    [UsedImplicitly]
    public class IntroSceneContext : GanContext
    {
        public UIManager      UIManager    { get; set; }
        public SceneManagerEx SceneManager { get; set; }

        [UsedImplicitly]
        public void OnClickStart()
        {
            SceneManager?.LoadScene(Define.eScene.SIMPLE_DEMO);
        }

        [UsedImplicitly]
        public void OnClickNewGame()
        {
            GanDebugger.Log(GetType().Name, "OnClickNewGame");
        }

        [UsedImplicitly]
        public void OnClickSetting()
        {
            GanDebugger.Log(GetType().Name, "OnClickSetting");
        }

        [UsedImplicitly]
        public void OnClickExit()
        {
            UIManager?.SetPopupOkCancel("종료하기", "게임을 종료하시겠습니까?",
                                        () =>
#if UNITY_EDITOR
                                        {
                                            EditorApplication.isPlaying = false;
                                        }
#else
                {
                    UnityEngine.Application.Quit();
                }
#endif
            );
        }
    }
}