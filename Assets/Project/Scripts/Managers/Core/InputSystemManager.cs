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
        private GanshinActions _playerActions;
        private InputActionMap _inputActionMap;
            
        public void Init()
        {
            _playerActions  = new GanshinActions();
            InitActionMap(eActiomMap.PLAYER_MOVEMENT);
            
            _playerActions.PlayerMovement.Attack.performed += ctx => Debug.Log("Attack");
        }
        
        private void InitActionMap(eActiomMap actionMap)
        {
            _inputActionMap?.Disable();
            switch (actionMap)
            {
                case eActiomMap.PLAYER_MOVEMENT:
                    _inputActionMap = _playerActions.PlayerMovement;
                    break;
            }
            _inputActionMap?.Enable();
        }
    }
}
