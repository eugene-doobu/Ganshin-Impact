using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI.Space
{
    public class UIPlayerAvatarContext : UIRootBase
    {
        [SerializeField] private Define.ePlayerAvatar target;
        private readonly         PlayerManager _playerManager = ProjectManager.Instance.GetManager<PlayerManager>();

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