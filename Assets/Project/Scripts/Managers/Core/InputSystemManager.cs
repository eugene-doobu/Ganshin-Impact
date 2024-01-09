using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GanShin.InputSystem
{
    public enum eActiomMap
    {
        NONE,
        PLAYER_MOVEMENT,
    }

    [UsedImplicitly]
    public class InputSystemManager : ManagerBase
    {
        private InputSystemManager() { }
        
        private readonly Dictionary<eActiomMap, ActionMapBase> _actionMapDict = new();

        private Action<InputControl> _onAnyKeyInput;

        private GanshinActions _playerActions;
        private InputActionMap _inputActionMap;

        public ActionMapBase GetActionMap(eActiomMap type) => _actionMapDict[type];

        public event Action<InputControl> OnAnyKeyInput
        {
            add
            {
                _onAnyKeyInput -= value;
                _onAnyKeyInput += value;
            }
            remove => _onAnyKeyInput -= value;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            _playerActions = new GanshinActions();
            InitActionMapDict();
            ChangeActionMap(eActiomMap.PLAYER_MOVEMENT);

            UnityEngine.InputSystem.InputSystem.onAnyButtonPress.Call(anyKey => _onAnyKeyInput?.Invoke(anyKey));
        }

        public void ChangeActionMap(eActiomMap actionMap)
        {
            _inputActionMap?.Disable();
            _inputActionMap = _actionMapDict[actionMap].GetActionMap();
            _inputActionMap.Enable();
        }

        private void InitActionMapDict()
        {
            _actionMapDict.Add(eActiomMap.NONE, new ActionMapNone(_playerActions.None));
            _actionMapDict.Add(eActiomMap.PLAYER_MOVEMENT, new ActionMapPlayerMove(_playerActions.PlayerMovement));
        }
    }
}