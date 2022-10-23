using System;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace GanShin.InputSystem
{
    public class ActionMapPlayerMove : ActionMapBase
    {
        public Action<Vector2> OnMovement;
        public Action<Vector2> OnLook;
        public Action<bool>    OnDash;
        public Action<bool>    OnAttack;
        public Action          OnJump;
        public Action<bool>    OnBaseSkill;
        public Action<bool>    OnUltimateSkill;
        public Action<bool>    OnInteraction;
        public Action          OnShortcutNumber1;
        public Action          OnShortcutNumber2;
        public Action          OnShortcutNumber3;
        public Action          OnShortcutNumber4;
        public Action          OnShortcutNumber5;
        public Action          OnShortcutNumber6;
        public Action          OnShortcutNumber7;
        public Action          OnShortcutNumber8;
        public Action          OnShortcutNumber9;
        public Action          OnShortcutNumber0;
        
        public ActionMapPlayerMove(GanshinActions.PlayerMovementActions actionMap) : base(actionMap)
        {
            actionMap.Movement.performed        += OnMovementPerformed;
            actionMap.Movement.canceled         += OnMovementCanceled;
            actionMap.Look.performed            += OnLookPerformed;
            actionMap.Look.canceled             += OnLookCanceled;
            actionMap.Dash.performed            += OnDashPerformed;
            actionMap.Dash.canceled             += OnDashCanceled;
            actionMap.Attack.performed          += OnAttackPerformed;
            actionMap.Attack.canceled           += OnAttackCanceled;
            actionMap.Jump.performed            += OnJumpPerformed;
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
            OnMovement?.Invoke(context.ReadValue<Vector2>());
        }
        
        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            OnMovement?.Invoke(Vector2.zero);
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            OnLook?.Invoke(context.ReadValue<Vector2>());
        }
        
        private void OnLookCanceled(InputAction.CallbackContext context)
        {
            OnLook?.Invoke(Vector2.zero);
        }
        
        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            OnDash?.Invoke(true);
        }
        
        private void OnDashCanceled(InputAction.CallbackContext context)
        {
            OnDash?.Invoke(false);
        }
        
        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            OnAttack?.Invoke(true);
        }
        
        public void OnAttackCanceled(InputAction.CallbackContext context)
        {
            OnAttack?.Invoke(false);
        }
        
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }
        
        private void OnBaseSkillPerformed(InputAction.CallbackContext context)
        {
            OnBaseSkill?.Invoke(true);
        }
        
        private void OnBaseSkillCanceled(InputAction.CallbackContext context)
        {
            OnBaseSkill?.Invoke(false);
        }
        
        private void OnUltimateSkillPerformed(InputAction.CallbackContext context)
        {
            OnUltimateSkill?.Invoke(true);
        }
        
        private void OnUltimateSkillCanceled(InputAction.CallbackContext context)
        {
            OnUltimateSkill?.Invoke(false);
        }
        
        private void OnInteractionPerformed(InputAction.CallbackContext context)
        {
            OnInteraction?.Invoke(true);
        }
        
        private void OnInteractionCanceled(InputAction.CallbackContext context)
        {
            OnInteraction?.Invoke(false);
        }
        
        private void OnShortcutNumber1Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber1?.Invoke();
        }
        
        private void OnShortcutNumber2Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber2?.Invoke();
        }
        
        private void OnShortcutNumber3Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber3?.Invoke();
        }
        
        private void OnShortcutNumber4Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber4?.Invoke();
        }
        
        private void OnShortcutNumber5Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber5?.Invoke();
        }
        
        private void OnShortcutNumber6Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber6?.Invoke();
        }
        
        private void OnShortcutNumber7Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber7?.Invoke();
        }
        
        private void OnShortcutNumber8Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber8?.Invoke();
        }
        
        private void OnShortcutNumber9Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber9?.Invoke();
        }
        
        private void OnShortcutNumber0Performed(InputAction.CallbackContext context)
        {
            OnShortcutNumber0?.Invoke();
        }
    }
}
