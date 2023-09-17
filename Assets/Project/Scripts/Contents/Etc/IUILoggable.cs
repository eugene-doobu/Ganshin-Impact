using GanShin.Space.UI;
using GanShin.UI;

namespace GanShin.Space.Content
{
    public interface IUILoggable
    {
        void UILog(string log, UIManager uiManager)
        {
            var logContext = uiManager.GetContext<LogContext>();
            logContext.Items.Add(new LogItemContext(log));
        }
    }
}
