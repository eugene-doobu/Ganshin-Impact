using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI.Space
{
    public class UIPlayerAvatarContext : UIRootBase
    {
        [SerializeField] private ePlayerAvatar target;
        private readonly         PlayerManager _playerManager = ProjectManager.Instance.GetManager<PlayerManager>();

        protected override Context InitializeDataContext()
        {
            return target switch
            {
                ePlayerAvatar.RIKO       => _playerManager?.GetAvatarContext(ePlayerAvatar.RIKO),
                ePlayerAvatar.AI         => _playerManager?.GetAvatarContext(ePlayerAvatar.AI),
                ePlayerAvatar.MUSCLE_CAT => _playerManager?.GetAvatarContext(ePlayerAvatar.MUSCLE_CAT),
                _                               => null
            };
        }
    }
}