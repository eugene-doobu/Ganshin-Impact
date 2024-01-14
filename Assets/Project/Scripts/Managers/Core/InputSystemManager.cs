using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GanShin.InputSystem
{
    public enum eActionMap
    {
        NONE,
        PLAYER_MOVEMENT
    }

    [UsedImplicitly]
    public class InputSystemManager : ManagerBase
    {
        private readonly Dictionary<eActionMap, ActionMapBase> _actionMapDict = new();
        private          InputActionMap                        _inputActionMap;

        private Action<InputControl> _onAnyKeyInput;

        private GanshinActions _playerActions;

        [UsedImplicitly]
        public InputSystemManager()
        {
        }

        public ActionMapBase GetActionMap(eActionMap type)
        {
            return _actionMapDict[type];
        }

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
            ChangeActionMap(eActionMap.PLAYER_MOVEMENT);

            UnityEngine.InputSystem.InputSystem.onAnyButtonPress.Call(anyKey => _onAnyKeyInput?.Invoke(anyKey));
        }

        public void ChangeActionMap(eActionMap actionMap)
        {
            _inputActionMap?.Disable();
            _inputActionMap = _actionMapDict[actionMap].GetActionMap();
            _inputActionMap.Enable();
        }

        private void InitActionMapDict()
        {
            _actionMapDict.Add(eActionMap.NONE, new ActionMapNone(_playerActions.None));
            _actionMapDict.Add(eActionMap.PLAYER_MOVEMENT, new ActionMapPlayerMove(_playerActions.PlayerMovement));
        }
    }
}