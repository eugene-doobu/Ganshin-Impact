using System;
using System.Collections;
using System.Collections.Generic;
using GanShin;
using UnityEngine;
using GanShin.InputSystem;

namespace GanShinTest.CoreMono
{
    public class InputTestMono : MonoBehaviour
    {
        private void Start()
        {
            var actionMap = Managers.Input.GetActionMap(eActiomMap.PLAYER_MOVEMENT);
            if (actionMap is not ActionMapPlayerMove playerMovement) return;
            
            playerMovement.OnMovement        += value =>Debug.Log("Movement" + value);
            playerMovement.OnLook            += value =>Debug.Log("Look" + value);
            playerMovement.OnDash            += value =>Debug.Log("Dash" + value);
            playerMovement.OnAttack          += value =>Debug.Log("Attack" + value);
            playerMovement.OnJump            += () =>Debug.Log("Jump");
            playerMovement.OnBaseSkill       += value =>Debug.Log("BaseSkill" + value);
            playerMovement.OnUltimateSkill   += value =>Debug.Log("UltimateSkill" + value);
            playerMovement.OnInteraction     += value =>Debug.Log("Interaction" + value);
            playerMovement.OnShortcutNumber1 += () =>Debug.Log("ShortcutNumber1");
            playerMovement.OnShortcutNumber2 += () =>Debug.Log("ShortcutNumber2");
            playerMovement.OnShortcutNumber3 += () =>Debug.Log("ShortcutNumber3");
            playerMovement.OnShortcutNumber4 += () =>Debug.Log("ShortcutNumber4");
            playerMovement.OnShortcutNumber5 += () =>Debug.Log("ShortcutNumber5");
            playerMovement.OnShortcutNumber6 += () =>Debug.Log("ShortcutNumber6");
            playerMovement.OnShortcutNumber7 += () =>Debug.Log("ShortcutNumber7");
            playerMovement.OnShortcutNumber8 += () =>Debug.Log("ShortcutNumber8");
            playerMovement.OnShortcutNumber9 += () =>Debug.Log("ShortcutNumber9");
            playerMovement.OnShortcutNumber0 += () =>Debug.Log("ShortcutNumber0");
        }

        void Update()
        {
            if (Input.GetKeyDown("["))
            {
                Managers.Input.ChangeActionMap(eActiomMap.NONE);
            }
            if (Input.GetKeyDown("]"))
            {
                Managers.Input.ChangeActionMap(eActiomMap.PLAYER_MOVEMENT);
            }
        }
    }
}
