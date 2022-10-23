using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GanShin.InputSystem
{
    public enum eActiomMap
    {
        NONE,
        PLAYER_MOVEMENT,
    }
    
    public class InputSystemManager
    {
        // TODO: AnyKeyInput 추가
        
        private readonly Dictionary<eActiomMap, ActionMapBase> _actionMapDict = new ();
        
        
        private GanshinActions _playerActions;
        private InputActionMap _inputActionMap;
        
        public ActionMapBase GetActionMap(eActiomMap type) => _actionMapDict[type];
            
        public void Init()
        {
            _playerActions  = new GanshinActions();
            InitActionMapDict();
            ChangeActionMap(eActiomMap.PLAYER_MOVEMENT);
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
