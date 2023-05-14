using System.ComponentModel;
using Zenject;

namespace GanShin.UI
{
    public class UIPlayerContext : UIRootBase
    {
        [Inject] private PlayerManager _playerManager;
        
        protected override INotifyPropertyChanged InitializeDataContext()
        {
            return _playerManager?.PlayerContext;
        }
    }
}
