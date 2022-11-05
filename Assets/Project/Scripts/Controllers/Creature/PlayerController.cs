using System;
using GanShin.InputSystem;
using UnityEngine;

namespace GanShin.Creature
{
    // TODO: abstract class로 변환 후 각 캐릭터별로 클래스를 구현해서 사용예정
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : CreatureController
    {
        #region Static

        private static int ANIM_PRAM_HASH_MOVE_FORWARD = UnityEngine.Animator.StringToHash("MoveForward");
        private static int ANIM_PRAM_HASH_MOVE_RIGHT   = UnityEngine.Animator.StringToHash("MoveRight");
        
        #endregion Static
        
        #region Variables
        
        private InputSystemManager  _input;
        private CharacterController _characterController;
        
        private Camera _camera; // TODO: CameraManager 활용 예정

        private float _jumpTimeoutDelta;
        private float _attackTimeoutDelta;

        private Vector2 _lastMovementValue;

        private float _moveAnimSmoothFactor = 4f;
        private float _moveForwardAnimValue;
        private float _moveRightAnimValue;
        
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
                GanDebugger.LogError(nameof(PlayerController),"actionMap is null!");
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
            MovementAnimation();
            if (_lastMovementValue == Vector2.zero) return;

            var direction = new Vector3(_lastMovementValue.x, 0, _lastMovementValue.y);
            _characterController.Move(direction * moveSpeed * Time.deltaTime);
        }

        private void MovementAnimation()
        {
            if (!HasAnimator) return;
            
            _moveForwardAnimValue = Mathf.Lerp(_moveForwardAnimValue, _lastMovementValue.y, _moveAnimSmoothFactor * Time.deltaTime);
            _moveRightAnimValue   = Mathf.Lerp(_moveRightAnimValue, _lastMovementValue.x, _moveAnimSmoothFactor * Time.deltaTime);
            
            Animator.SetFloat(ANIM_PRAM_HASH_MOVE_FORWARD, _moveForwardAnimValue);
            Animator.SetFloat(ANIM_PRAM_HASH_MOVE_RIGHT, _moveRightAnimValue);
        }

        #endregion Movement
        
        #region ActionEvent

        /// 이동류: 모든 캐릭터가 동일한 로직을 사용, 스텟의 차이만 있음
         
        protected virtual void OnMovement(Vector2 value)
        {
            _lastMovementValue = value;
        }
        
        protected virtual void OnDash(bool value)
        {
            
        }
        
        protected virtual void OnInteraction(bool value)
        {
        }
        
        protected virtual void OnJump()
        {
        }
        
        // 공격류: 캐릭터마다 다른 특징을 가지고 있으므로 가상함수로 구현
        
        protected virtual void OnAttack(bool value)
        {               
        }
        
        protected virtual void OnBaseSkill(bool value)
        {
        }
        
        protected virtual void OnUltimateSkill(bool value)
        {
        }
        
        #endregion ActionEvent
    }
}
