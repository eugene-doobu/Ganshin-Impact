using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
    public class UI_Popup : UI_Base
    {
        [Inject] private UIManager _ui;
        
        public override void Init()
        {
            _ui.SetCanvas(gameObject, true);
        }

        public virtual void ClosePopupUI()
        {
            _ui.ClosePopupUI(this);
        }
    }
}

