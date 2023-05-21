using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.UI
{
    public abstract class GlobalUIRootBase : UIRootBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public abstract void InitializeContextData();

        public abstract void ClearContextData();
    }
}