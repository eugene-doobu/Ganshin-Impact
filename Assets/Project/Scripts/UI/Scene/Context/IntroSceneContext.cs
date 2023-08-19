using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class IntroSceneContext : Context
    {
        public void OnClickStart()
        {
            GanDebugger.Log(GetType().Name,"OnClickStart");
        }
        
        public void OnClickNewGame()
        {
            GanDebugger.Log(GetType().Name,"OnClickNewGame");
        }
        
        public void OnClickSetting()
        {
            GanDebugger.Log(GetType().Name,"OnClickSetting");
        }

        public void OnClickExit()
        {
            GanDebugger.Log(GetType().Name,"OnClickExit");
        }
    }
}
