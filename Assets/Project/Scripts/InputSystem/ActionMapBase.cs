using UnityEngine.InputSystem;

namespace GanShin.InputSystem
{
    public abstract class ActionMapBase
    {
        protected ActionMapBase(InputActionMap actionMap)
        {
            ActionMap = actionMap;
        }

        protected InputActionMap ActionMap { get; set; }

        public InputActionMap GetActionMap()
        {
            return ActionMap;
        }
    }
}