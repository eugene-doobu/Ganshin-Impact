using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    public class UIPlayerContext : UIRootBase
    {
        [Inject] private PlayerManager _playerManager;
        
        protected override Context InitializeDataContext()
        {
            return _playerManager?.PlayerContext;
        }
    }
}
