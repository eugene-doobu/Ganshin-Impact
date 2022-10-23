using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GanShin.InputSystem
{
    public enum eActiomMap
    {
        PLAYER_MOVEMENT,
    }
    
    public class InputSystemManager
    {
        private readonly Dictionary<eActiomMap, ActionMapBase> _actionMapDict = new ();

        private GanshinActions _playerActions;
        private InputActionMap _inputActionMap;
            
        public void Init()
        {
            _playerActions  = new GanshinActions();
            InitActionMapDict();
            ChangeActionMap(eActiomMap.PLAYER_MOVEMENT);

            _inputActionMap = _playerActions.PlayerMovement;
            
            _playerActions.PlayerMovement.Attack.performed += ctx => Debug.Log("Attack");
        }
        
        private void ChangeActionMap(eActiomMap actionMap)
        {
            _inputActionMap?.Disable();
            _inputActionMap = _actionMapDict[actionMap].GetActionMap();
            _inputActionMap.Enable();
        }

        private void InitActionMapDict()
        {
            _actionMapDict.Add(eActiomMap.PLAYER_MOVEMENT, new ActionMapPlayerMove(_playerActions.PlayerMovement));
        }
    }
}
