using System;
using UnityEngine.InputSystem;

namespace GanShin.InputSystem
{
    public abstract class ActionMapBase
    {
        protected InputActionMap ActionMap { get; set; }

        public InputActionMap GetActionMap() => ActionMap;

        protected ActionMapBase(InputActionMap actionMap)
        {
            ActionMap = actionMap;
        }
    }
}