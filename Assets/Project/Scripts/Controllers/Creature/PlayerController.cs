using System;
using GanShin.InputSystem;
using UnityEngine;

namespace GanShin.Creature
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : CreatureController
    {
        #region Variables
        
        private InputSystemManager  _input;
        private CharacterController _characterController;
        
        private Camera   _camera; // TODO: CameraManager 활용 예정

        private float _jumpTimeoutDelta;
        private float _attackTimeoutDelta;

        private Vector2 _lastMovementValue;
        
        #endregion Variables
        

        #region Mono

        protected override void Awake()
        {
            base.Awake();
            
            _input       = Managers.Input;
            _camera      = Camera.main;
            
            _characterController = GetComponent<CharacterController>();

            AddInputEvent();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void OnDestroy()
        {
            RemoveInputEvent();
        }

        #endregion Mono

        protected virtual void LoadData()
        {
            // Json 형식의 데이터를 읽어서 해당 캐릭터의 스텟을 설정
        }
        
        private void AddInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
            {
                Debug.LogError($"[{nameof(Creature)}] actionMap is null!");
                return;
            }

            actionMap.OnAttack        += OnAttack;
            actionMap.OnDash          += OnDash;
            actionMap.OnInteraction   += OnInteraction;
            actionMap.OnJump          += OnJump;
            actionMap.OnMovement      += OnMovement;
            actionMap.OnBaseSkill     += OnBaseSkill;
            actionMap.OnUltimateSkill += OnUltimateSkill;
        }

        private void RemoveInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
                return;

            actionMap.OnAttack        -= OnAttack;
            actionMap.OnDash          -= OnDash;
            actionMap.OnInteraction   -= OnInteraction;
            actionMap.OnJump          -= OnJump;
            actionMap.OnMovement      -= OnMovement;
            actionMap.OnBaseSkill     -= OnBaseSkill;
            actionMap.OnUltimateSkill -= OnUltimateSkill;
        }

        #region Movement

        protected override void Movement(float moveSpeed)
        {
            if (_lastMovementValue == Vector2.zero) return;
            
            // if (_hasAnimator && value == Vector2.zero)
            // {
            //     _animator.SetBool("IsMove", false);
            //     return;
            // }
            //
            // _animator.SetBool("IsMove", true);

            var direction = new Vector3(_lastMovementValue.x, 0, _lastMovementValue.y);
            _characterController.Move(direction * moveSpeed * Time.deltaTime);
        }

        private void MovementAnimation()
        {
            if (!HasAnimator) return;
        }

        #endregion Movement
        
        #region ActionEvent

        /// 이동류: 모든 캐릭터가 동일한 로직을 사용, 스텟의 차이만 있음
         
        protected virtual void OnMovement(Vector2 value)
        {
            Debug.Log($"[{nameof(Creature)}] OnMovement - value: {value}");
            _lastMovementValue = value;
        }
        
        protected virtual void OnDash(bool value)
        {
            Debug.Log($"[{nameof(Creature)}] OnDash - value: {value}");
        }
        
        protected virtual void OnInteraction(bool value)
        {
            Debug.Log($"[{nameof(Creature)}] OnInteraction - value: {value}");
        }
        
        protected virtual void OnJump()
        {
            Debug.Log($"[{nameof(Creature)}] OnJump");
        }
        
        // 공격류: 캐릭터마다 다른 특징을 가지고 있으므로 가상함수로 구현
        
        protected virtual void OnAttack(bool value)
        {               
            Debug.Log($"[{nameof(Creature)}] OnAttack - value: {value}");
        }
        
        protected virtual void OnBaseSkill(bool value)
        {
            Debug.Log($"[{nameof(Creature)}] OnBaseSkill - value: {value}");
        }
        
        protected virtual void OnUltimateSkill(bool value)
        {
            Debug.Log($"[{nameof(Creature)}] OnUltimateSkill - value: {value}");
        }
        
        #endregion ActionEvent
    }
}
