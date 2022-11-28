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

        private static int ANIM_PRAM_HASH_ISMOVE     = Animator.StringToHash("IsMove");
        private static int ANIM_PRAM_HASH_MOVE_SPEED = Animator.StringToHash("MoveSpeed");
        
        #endregion Static
        
        #region Variables
        
        private InputSystemManager  _input;
        private CharacterController _characterController;

        private Transform _tr;
        
        private Vector2 _lastMovementValue;

        [Header("Movement")]
        [SerializeField] 
        private float _moveAnimSmoothFactor = 4f;
        private float _moveAnimValue;

        [SerializeField] 
        private float _rotationSmoothFactor = 8f;
        
        [Header("Jump")]
        [SerializeField] 
        private float _jumpPower    = 5f;
        [SerializeField] 
        private float _jumpCooldown = 0.5f;
        
        private float _jumpTimer    = 0f;
        private float _jumpVelocity = 0f;
        private bool  _desiredJump  = false;
        [SerializeField]
        private float _gravity      = -9.8f;
        
        #endregion Variables
        

        #region Mono

        protected override void Awake()
        {
            base.Awake();
            
            _input               = Managers.Input;
            _characterController = GetComponent<CharacterController>();
            _tr                  = GetComponent<Transform>();

            AddInputEvent();
        }

        protected override void Update()
        {
            base.Update();
            Jump();
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

            var mainCamera    = Managers.Camera.MainCamera;
            var cameraForward = Vector3.forward;
            var cameraRight   = Vector3.right;
            if (!ReferenceEquals(mainCamera, null))
            {
                cameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
                cameraRight   = Vector3.Cross(Vector3.up, cameraForward);
            }

            var direction = (cameraForward * _lastMovementValue.y + cameraRight * _lastMovementValue.x).normalized;
            _characterController.Move(direction * moveSpeed * Time.deltaTime);

            var targetRotation = Quaternion.LookRotation(direction);
            _tr.rotation = Quaternion.Slerp(_tr.rotation, targetRotation, _rotationSmoothFactor * Time.deltaTime);
        }

        private void MovementAnimation()
        {
            if (!HasAnimator) return;
            
            _moveAnimValue = Mathf.Lerp(_moveAnimValue, _lastMovementValue.magnitude, _moveAnimSmoothFactor * Time.deltaTime);
            
            Animator.SetBool(ANIM_PRAM_HASH_ISMOVE, _lastMovementValue != Vector2.zero);
            Animator.SetFloat(ANIM_PRAM_HASH_MOVE_SPEED, _moveAnimValue);
        }

        private void Jump()
        {
            _jumpTimer -= Time.deltaTime;
            _jumpTimer =  Mathf.Clamp(_jumpTimer, -1f, _jumpTimer);
            
            var value = _desiredJump;
            _desiredJump = false;
            
            _jumpVelocity += _gravity * Time.deltaTime;

            if (value && _jumpTimer <= 0f)
            {
                _jumpVelocity = Mathf.Sqrt(_jumpPower * -_gravity);
                _jumpTimer    =  _jumpCooldown;
            }
            
            _characterController.Move(Vector3.up * _jumpVelocity * Time.deltaTime);
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
            if (_characterController.isGrounded)
                _desiredJump = true;
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
