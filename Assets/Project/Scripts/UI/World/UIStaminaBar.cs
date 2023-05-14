using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
    public class UIStaminaBar : UIRootBase
    {
        [Inject] private PlayerManager _playerManager;
        
        protected override INotifyPropertyChanged InitializeDataContext()
        {
            return _playerManager?.PlayerContext;
        }
    }
}
