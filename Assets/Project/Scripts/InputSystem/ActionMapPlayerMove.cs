using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GanShin.InputSystem
{
    public class ActionMapPlayerMove : ActionMapBase
    {
        public Action<bool>    OnAttack;
        public Action<bool>    OnBaseSkill;
        public Action<bool>    OnInteraction;
        public Action<Vector2> OnLook;
        public Action<Vector2> OnMovement;
        public Action          OnRoll;
        public Action          OnShortcutNumber0;
        public Action          OnShortcutNumber1;
        public Action          OnShortcutNumber2;
        public Action          OnShortcutNumber3;
        public Action          OnShortcutNumber4;
        public Action          OnShortcutNumber5;
        public Action          OnShortcutNumber6;
        public Action          OnShortcutNumber7;
        public Action          OnShortcutNumber8;
        public Action          OnShortcutNumber9;
        public Action<bool>    OnSpecialAction;
        public Action<bool>    OnUltimateSkill;
        public Action<float>   OnZoom;

        public ActionMapPlayerMove(GanshinActions.PlayerMovementActions actionMap) : base(actionMap)
        {
            actionMap.Movement.performed        += OnMovementPerformed;
            actionMap.Movement.canceled         += OnMovementCanceled;
            actionMap.Look.performed            += OnLookPerformed;
            actionMap.Look.canceled             += OnLookCanceled;
            actionMap.SpecialAction.performed   += OnSpecialActionPerformed;
            actionMap.SpecialAction.canceled    += OnSpecialActionCanceled;
            actionMap.Attack.performed          += OnAttackPerformed;
            actionMap.Attack.canceled           += OnAttackCanceled;
            actionMap.Roll.performed            += OnRollPerformed;
            actionMap.Zoom.performed            += OnZoomPerformed;
            actionMap.Zoom.canceled             += OnZoomCanceled;
            actionMap.BaseSkill.performed       += OnBaseSkillPerformed;
            actionMap.BaseSkill.canceled        += OnBaseSkillCanceled;
            actionMap.UltimateSkill.performed   += OnUltimateSkillPerformed;
            actionMap.UltimateSkill.canceled    += OnUltimateSkillCanceled;
            actionMap.Interaction.performed     += OnInteractionPerformed;
            actionMap.Interaction.canceled      += OnInteractionCanceled;
            actionMap.ShortcutNumber1.performed += OnShortcutNumber1Performed;
            actionMap.ShortcutNumber2.performed += OnShortcutNumber2Performed;
            actionMap.ShortcutNumber3.performed += OnShortcutNumber3Performed;
            actionMap.ShortcutNumber4.performed += OnShortcutNumber4Performed;
            actionMap.ShortcutNumber5.performed += OnShortcutNumber5Performed;
            actionMap.ShortcutNumber6.performed += OnShortcutNumber6Performed;
            actionMap.ShortcutNumber7.performed += OnShortcutNumber7Performed;
            actionMap.ShortcutNumber8.performed += OnShortcutNumber8Performed;
            actionMap.ShortcutNumber9.performed += OnShortcutNumber9Performed;
            actionMap.ShortcutNumber0.performed += OnShortcutNumber0Performed;
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            OnMovement?.Invoke(value);
            GanDebugger.InputLog($"OnMovement - value: {value}");
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            var value = Vector2.zero;
            OnMovement?.Invoke(value);
            GanDebugger.InputLog($"OnMovement - value: {value}");
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            OnLook?.Invoke(value);
            GanDebugger.InputLog($"OnLook - value: {value}");
        }

        private void OnLookCanceled(InputAction.CallbackContext context)
        {
            OnLook?.Invoke(Vector2.zero);
            GanDebugger.InputLog($"OnLook - value: {Vector2.zero}");
        }

        private void OnSpecialActionPerformed(InputAction.CallbackContext context)
        {
            OnSpecialAction?.Invoke(true);
            GanDebugger.InputLog($"OnSpecialAction - value: {true}");
        }

        private void OnSpecialActionCanceled(InputAction.CallbackContext context)
        {
            OnSpecialAction?.Invoke(false);
            GanDebugger.InputLog($"OnSpecialAction - value: {false}");
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem != null && eventSystem.IsPointerOverGameObject())
                return;

            OnAttack?.Invoke(true);
            GanDebugger.InputLog($"OnAttack - value: {true}");
        }

        private void OnAttackCanceled(InputAction.CallbackContext context)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem != null && eventSystem.IsPointerOverGameObject())
                return;

            OnAttack?.Invoke(false);
            GanDebugger.InputLog($"OnAttack - value: {false}");
        }

        private void OnRollPerformed(InputAction.CallbackContext context)
        {
            OnRoll?.Invoke();
            GanDebugger.InputLog($"OnRoll - value: {true}");
        }

        private void OnZoomPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            OnZoom?.Invoke(value);
            GanDebugger.InputLog($"OnZoom - value: {value}");
        }

        private void OnZoomCanceled(InputAction.CallbackContext context)
        {
            OnZoom?.Invoke(0);
            GanDebugger.InputLog("OnZoom - value: 0");
        }

        private void OnBaseSkillPerformed(InputAction.CallbackContext context)
        {
            OnBaseSkill?.Invoke(true);
            GanDebugger.InputLog($"OnBaseSkill - value: {true}");
        }

        private void OnBaseSkillCanceled(InputAction.CallbackContext context)
        {
            OnBaseSkill?.Invoke(false);
            GanDebugger.InputLog($"OnBaseSkill - value: {false}");
        }

        private void OnUltimateSkillPerformed(InputAction.CallbackContext context)
        {
            OnUltimateSkill?.Invoke(true);
            GanDebugger.InputLog($"OnUltimateSkill - value: {true}");
        }

        private void OnUltimateSkillCanceled(InputAction.CallbackContext context)
        {
            OnUltimateSkill?.Invoke(false);
            GanDebugger.InputLog($"OnUltimateSkill - value: {false}");
        }

        private void OnInteractionPerformed(InputAction.CallbackContext context)
        {
            OnInteraction?.Invoke(true);
            GanDebugger.InputLog($"OnInteraction - value: {true}");
        }

        private void OnInteractionCanceled(InputAction.CallbackContext context)
        {
            OnInteraction?.Invoke(false);
            GanDebugger.InputLog($"OnInteraction - value: {false}");
        }

        private void OnShortcutNumber1Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber1?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber1 - value: {true}");
        }

        private void OnShortcutNumber2Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber2?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber2 - value: {true}");
        }

        private void OnShortcutNumber3Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber3?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber3 - value: {true}");
        }

        private void OnShortcutNumber4Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber4?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber4 - value: {true}");
        }

        private void OnShortcutNumber5Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber5?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber5 - value: {true}");
        }

        private void OnShortcutNumber6Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber6?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber6 - value: {true}");
        }

        private void OnShortcutNumber7Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber7?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber7 - value: {true}");
        }

        private void OnShortcutNumber8Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber8?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber8 - value: {true}");
        }

        private void OnShortcutNumber9Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber9?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber9 - value: {true}");
        }

        private void OnShortcutNumber0Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber0?.Invoke();
            GanDebugger.InputLog($"OnShortcutNumber0 - value: {true}");
        }
    }
}