using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI.Space
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