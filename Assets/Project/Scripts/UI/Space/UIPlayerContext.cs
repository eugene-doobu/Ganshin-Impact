using GanShin.UI;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Space.UI
{
    public class UIPlayerContext : UIRootBase
    {
        private PlayerManager _playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
        
        protected override Context InitializeDataContext()
        {
            return _playerManager?.PlayerContext;
        }
    }
}
