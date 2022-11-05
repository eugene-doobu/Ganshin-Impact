using System.Collections;
using System.Collections.Generic;
using GanShin.InputSystem;

namespace GanShin.CameraSystem
{
    public class AvatarCamera : CameraBase
    {
        private InputSystemManager _input;
        
        public AvatarCamera()
        {
            _input = Managers.Input;
        }
    
        public override void OnEnable()
        {
            base.OnEnable();
            AddInputEvent();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        
        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
        }
        
        public override void OnDisable()
        {
            RemoveInputEvent();
            base.OnDisable();
        }

        #region Input

        private void AddInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
            {
                GanDebugger.CameraLogError("actionMap is null!");
                return;
            }
        }

        private void RemoveInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
                return;
        }
        

        #endregion Input
    }
}
