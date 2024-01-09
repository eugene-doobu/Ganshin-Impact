using GanShin.UI;
using UnityEngine;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Space.UI
{
    public class UIPlayerAvatarContext : UIRootBase
    {
        private PlayerManager _playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
        
        [SerializeField] private Define.ePlayerAvatar target;
        
        protected override Context InitializeDataContext()
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
