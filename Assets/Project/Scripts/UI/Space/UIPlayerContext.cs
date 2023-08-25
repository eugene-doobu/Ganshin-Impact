using GanShin.UI;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Space.UI
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
