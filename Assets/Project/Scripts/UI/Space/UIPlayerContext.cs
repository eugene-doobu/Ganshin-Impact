using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    public class UIPlayerContext : UIRootBase
    {
        private readonly PlayerManager _playerManager = ProjectManager.Instance.GetManager<PlayerManager>();

        protected override Context InitializeDataContext()
        {
            return _playerManager?.PlayerContext;
        }
    }
}