using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
    public enum eHpTarget
    {
        RIKO,
        AI,
        MUSCLE_CAT,
        SELF, // 자신의 contextHolder를 이용하는 경우
    }
    
    public class UIHpBar : UIRootBase
    {
        [Inject] private PlayerManager _playerManager;
        
        [SerializeField]
        private eHpTarget target;
        
        protected override INotifyPropertyChanged InitializeDataContext()
        {
            switch (target)
            {
                case eHpTarget.RIKO:
                    return _playerManager?.GetUIHpBarContext(Define.ePlayerAvatar.RIKO);
                case eHpTarget.AI:
                    return _playerManager?.GetUIHpBarContext(Define.ePlayerAvatar.AI);
                case eHpTarget.MUSCLE_CAT:
                    return _playerManager?.GetUIHpBarContext(Define.ePlayerAvatar.MUSCLE_CAT);
                case eHpTarget.SELF:
                default:
                    return new UIHpBarContext();
            }
        }

        private UIHpBarContext _context;
        
        public UIHpBarContext Context => _context ?? DataContext as UIHpBarContext;
    }
}
