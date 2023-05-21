using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
    public class UIPlayerAvatarContext : UIRootBase
    {
        [Inject] private PlayerManager _playerManager;
        
        [SerializeField] private Define.ePlayerAvatar target;
        
        protected override INotifyPropertyChanged InitializeDataContext()
        {
            return target switch
            {
                Define.ePlayerAvatar.RIKO       => _playerManager?.GetAvatarContext(Define.ePlayerAvatar.RIKO),
                Define.ePlayerAvatar.AI         => _playerManager?.GetAvatarContext(Define.ePlayerAvatar.AI),
                Define.ePlayerAvatar.MUSCLE_CAT => _playerManager?.GetAvatarContext(Define.ePlayerAvatar.MUSCLE_CAT),
                _                               => null
            };
        }
    }
}
