using System;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.InputSystem;
using UnityEngine;
using Zenject;

namespace GanShin.Creature
{
    // TODO: abstract class로 변환 후 각 캐릭터별로 클래스를 구현해서 사용예정
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : CreatureController
    {
        #region Static

        private static int ANIM_PRAM_HASH_ISMOVE     = Animator.StringToHash("IsMove");
        private static int ANIM_PRAM_HASH_MOVE_SPEED = Animator.StringToHash("MoveSpeed");
        private static int ANIM_PRAM_HASH_ROLL_START = Animator.StringToHash("RollStart");
        
        #endregion Static
        
        #region Variables
        
        [Inject] private InputSystemManager _input;
        [Inject] private CameraManager      _camera;
        
        private CharacterController _characterController;

        private Transform _tr;
        
        private Vector2 _lastMovementValue;

        private float _moveAnimValue;

        [SerializeField] 
        private float rotationSmoothFactor = 8f;
        
        [SerializeField] 
        private float rollCooldown = 0.5f;
         
        [SerializeField]
        private float _gravity = -1f;
        
        private bool _canRoll = true;
        private bool _desiredRoll;

        [Space] 
        [Header("GroundCheck")]
        [SerializeField]
        private float _rayStartPosOffset = 0.3f;
        [SerializeField] 
        private float _groundCheckDistance = 0.5f;
        [SerializeField] 
        private float _groundCheckRadius = 0.1f;
        [SerializeField] 
        private LayerMask _groundLayerMask;

        private bool _isOnGround;
        
        #endregion Variables

        #region Mono

        protected override void Awake()
        {
            base.Awake();
            
            _characterController = GetComponent<CharacterController>();
            _tr                  = GetComponent<Transform>();

            AddInputEvent();
        }

        protected override void Update()
        {
            CheckOnGround();
            base.Update();
            Roll();
            ApplyGravity();
        }

        private void OnDestroy()
        {
            RemoveInputEvent();
        }

        #endregion Mono

        #region StateCheck

        protected void CheckOnGround()
        {
            var rayStartPos = _tr.position + Vector3.up * _rayStartPosOffset;
            if (Physics.SphereCast(rayStartPos, _groundCheckRadius, Vector3.down, out var hit, _groundCheckDistance, _groundLayerMask))
            {
                _isOnGround = true;
            }
            else
            {
                _isOnGround = false;
            }
        }

        #endregion StateCheck

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
            actionMap.OnRoll          += OnRoll;
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
            actionMap.OnRoll          -= OnRoll;
            actionMap.OnMovement      -= OnMovement;
            actionMap.OnBaseSkill     -= OnBaseSkill;
            actionMap.OnUltimateSkill -= OnUltimateSkill;
        }

        #region Movement

        protected override void Movement(float moveSpeed)
        {
            PlayMovementAnimation();
            if (_lastMovementValue == Vector2.zero) return;

            var mainCamera    = _camera.MainCamera;
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
            _tr.rotation = Quaternion.Slerp(_tr.rotation, targetRotation, rotationSmoothFactor * Time.deltaTime);
        }

        private void PlayMovementAnimation()
        {
            if (!HasAnimator) return;
            
            _moveAnimValue = Mathf.Lerp(_moveAnimValue, _lastMovementValue.magnitude, rotationSmoothFactor * Time.deltaTime);
            
            ObjAnimator.SetBool(ANIM_PRAM_HASH_ISMOVE, _lastMovementValue != Vector2.zero);
            ObjAnimator.SetFloat(ANIM_PRAM_HASH_MOVE_SPEED, _moveAnimValue);
        }
        
        private void Roll()
        {
            if (!_desiredRoll) return;
            _desiredRoll = false;
            
            if (!_canRoll) return;
            PlayRollAnimation();
            JumpTimer().Forget();
            _canRoll = false;
        }

        private async UniTask JumpTimer()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(rollCooldown));
            _canRoll = true;
        }

        private void PlayRollAnimation()
        {
            if (!HasAnimator) return;
            ObjAnimator.SetTrigger(ANIM_PRAM_HASH_ROLL_START);
        }

        private void ApplyGravity()
        {
            _characterController.Move(Vector3.up * _gravity * Time.deltaTime);
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
        
        protected virtual void OnRoll()
        {
            if (_isOnGround) _desiredRoll = true;
        }
        
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
