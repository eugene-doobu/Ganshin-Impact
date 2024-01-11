using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.Space.UI
{
    public enum eHpTarget
    {
        RIKO,
        AI,
        MUSCLE_CAT,
        OBJECT, // 특정 오브젝트의 Context를 가져오는 경우
        SELF    // 자신의 contextHolder를 이용하는 경우
    }

    public class UIHpBar : UIRootBase
    {
        [SerializeField] private eHpTarget target = eHpTarget.SELF;

        /// <summary>
        ///     target이 Object인경우 컨텍스트를 가지고 있는 오브젝트
        /// </summary>
        [SerializeField] private GameObject owner;

        private CreatureObjectContext _context;

        public CreatureObjectContext Context => _context ?? DataContext as CreatureObjectContext;

        protected override Context InitializeDataContext()
        {
            var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
            switch (target)
            {
                case eHpTarget.RIKO:
                    return playerManager?.GetAvatarContext(Define.ePlayerAvatar.RIKO);
                case eHpTarget.AI:
                    return playerManager?.GetAvatarContext(Define.ePlayerAvatar.AI);
                case eHpTarget.MUSCLE_CAT:
                    return playerManager?.GetAvatarContext(Define.ePlayerAvatar.MUSCLE_CAT);
                case eHpTarget.OBJECT:
                    if (owner == null)
                    {
                        GanDebugger.LogWarning(nameof(UIHpBar), "owner is null");
                        return null;
                    }

                    return owner.TryGetComponent(out IDataContextOwner contextOwner) ? contextOwner.DataContext : null;
                case eHpTarget.SELF:
                default:
                    return null;
            }
        }
    }
}