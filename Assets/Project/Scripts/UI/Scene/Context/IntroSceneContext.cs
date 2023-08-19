using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    [UsedImplicitly]
    public class IntroSceneContext : Context
    {
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
            GanDebugger.Log(GetType().Name,"OnClickExit");
        }
    }
}
