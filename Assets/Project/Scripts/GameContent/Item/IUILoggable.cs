using GanShin.UI;

namespace GanShin.Item
{
    public interface IUILoggable
    {
        void UILog(string log, UIManager uiManager)
        {
            uiManager.AddLog(log);
        }
    }
}