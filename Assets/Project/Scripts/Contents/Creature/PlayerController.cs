using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.InputSystem;
using GanShin.Utils;
using GanShin.Content.Weapon;
using GanShin.Data;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace GanShin.Content.Creature
{
    // TODO: abstract class로 변환 후 각 캐릭터별로 클래스를 구현해서 사용예정
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : CreatureController, IAttackAnimation
    {
#region Static
        private static int ANIM_PRAM_HASH_ISMOVE       = Animator.StringToHash("IsMove");
        private static int ANIM_PRAM_HASH_MOVE_SPEED   = Animator.StringToHash("MoveSpeed");
        private static int ANIM_PRAM_HASH_ROLL_START   = Animator.StringToHash("RollStart");
        private static int ANIM_PRAM_HASH_ATTACK_STATE = Animator.StringToHash("AttackState");
        private static int ANIM_PRAM_HASH_SET_IDLE     = Animator.StringToHash("SetIdle");
        private static int ANIM_PRAM_HASH_SET_DEAD     = Animator.StringToHash("SetDead");
#endregion Static

#region Variables
        [Inject] private InputSystemManager _input;
        [Inject] private CameraManager      _camera;
        [Inject] private PlayerManager      _playerManager;

        private CharacterController _characterController;
        private UIHpBarContext      _uiHpBarContext;

        private Transform _tr;
        private Transform _wristLeftTr;
        private Transform _wristRightTr;

        private Vector2 _lastMovementValue;

        private float _moveAnimValue;

        [SerializeField] private PlayerWeaponBase   weapon;
        [SerializeField] private CharacterStatTable stat;

        [SerializeField] private float rotationSmoothFactor = 8f;
        [SerializeField] private float rollCooldown         = 0.5f;
        [SerializeField] private float gravity              = -1f;

        private bool _canRoll = true;
        private bool _desiredRoll;

        [Space] [Header("GroundCheck")] [SerializeField]
        private float rayStartPosOffset = 0.3f;

        [SerializeField] private float     groundCheckDistance = 0.5f;
        [SerializeField] private float     groundCheckRadius   = 0.1f;
        [SerializeField] private LayerMask groundLayerMask;

        [Space]
        // TODO: Attack 관련 내용 별도의 클래스를 조합하여 관리
        [Header("Attack")]
        [SerializeField]
        private float attackCooldown = 0.2f;

        [SerializeField] private float attackToIdleTime = 1f;

        [SerializeField] [ReadOnly] private ePlayerAttack playerAttack;

        [SerializeField] private bool isOnUltimate;

        private bool _canAttack = true;
        private bool _desiredAttack;

        private bool _isOnGround;

        private float _staminaCost = 20f;

        private CancellationTokenSource _attackCancellationTokenSource;
        private bool                    _isOnAttack;
        private float                   _currentHp;
#endregion Variables

#region Properties
        public CharacterStatTable Stat => stat;

        public ePlayerAttack PlayerAttack
        {
            get => playerAttack;

            private set
            {
                if (playerAttack == value) return;
                playerAttack      = value;
                weapon.AttackType = value;
            }
        }

        public float CurrentHp
        {
            get => _currentHp;
            private set
            {
                if (Mathf.Approximately(_currentHp, value)) return;

                _currentHp = Mathf.Clamp(value, 0, stat.hp);

                _uiHpBarContext.CurrentHp = (int) _currentHp;
            }
        }
#endregion Properties

#region Mono
        protected override void Awake()
        {
            base.Awake();

            InitializeAvatar();
            InitializeWeapon();
            AddInputEvent();

            _uiHpBarContext = _playerManager.GetUIHpBarContext(Define.ePlayerAvatar.RIKO);
        }

        protected override void Start()
        {
            base.Start();
            // TODO: 매니저격 클래스에서 셋팅할 예정
            _camera.ChangeTarget(_tr);
            _uiHpBarContext.MaxHp = (int) stat.hp;

            CurrentHp = stat.hp;
        }

        protected override void Update()
        {
            CheckOnGround();
            base.Update();
            Roll();
            ApplyGravity();
            Attack();
        }

        private void OnDestroy()
        {
            RemoveInputEvent();
        }
#endregion Mono

#region StateCheck
        private void InitializeAvatar()
        {
            _characterController = GetComponent<CharacterController>();
            _tr                  = GetComponent<Transform>();

            _wristLeftTr  = _tr.RecursiveFind(AvatarDefine.WristLeftBone);
            _wristRightTr = _tr.RecursiveFind(AvatarDefine.WristRightBone);
        }

        private void InitializeWeapon()
        {
            weapon.Owner = this;
        }

        protected void CheckOnGround()
        {
            var rayStartPos = _tr.position + Vector3.up * rayStartPosOffset;
            if (Physics.SphereCast(rayStartPos, groundCheckRadius, Vector3.down, out var hit, groundCheckDistance,
                    groundLayerMask))
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
                GanDebugger.LogError(nameof(PlayerController), "actionMap is null!");
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

            _moveAnimValue = Mathf.Lerp(_moveAnimValue, _lastMovementValue.magnitude,
                rotationSmoothFactor * Time.deltaTime);

            ObjAnimator.SetBool(ANIM_PRAM_HASH_ISMOVE, _lastMovementValue != Vector2.zero);
            ObjAnimator.SetFloat(ANIM_PRAM_HASH_MOVE_SPEED, _moveAnimValue);
        }

        private void Roll()
        {
            if (!_desiredRoll) return;
            _desiredRoll = false;

            if (!_canRoll) return;
            
            if (_playerManager.CurrentStamina < _staminaCost) return;
            _playerManager.CurrentStamina -= _staminaCost;
            GanDebugger.LogWarning(_playerManager.CurrentStamina.ToString());
            
            PlayRollAnimation();
            DelayRoll().Forget();
            _canRoll = false;
        }

        private async UniTask DelayRoll()
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
            _characterController.Move(Vector3.up * gravity * Time.deltaTime);
        }
#endregion Movement

#region Attack
        // TODO: 캐릭터별로 다르게 처리 + 별도의 공격 클래스를 만들어서 처리
        private void Attack()
        {
            if (!_desiredAttack) return;
            _desiredAttack = false;

            if (!_canAttack) return;
            _canAttack = false;
            DelayAttack().Forget();

            bool _isTryAttack = false;
            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                    if (!isOnUltimate)
                    {
                        PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK1;
                        ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 1);
                    }
                    else
                    {
                        PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK1;
                        ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 5);
                    }

                    _isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK1:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK2;
                    ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 2);
                    _isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK2:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK3;
                    ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 3);
                    _isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK3:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK4;
                    ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 4);
                    _isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK1:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK2;
                    ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 6);
                    _isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK2:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK3;
                    ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 7);
                    _isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK3:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK4;
                    ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 8);
                    _isTryAttack = true;
                    break;
            }

            if (_isTryAttack)
            {
                if (_attackCancellationTokenSource != null)
                    DisposeAttackCancellationTokenSource();
                _attackCancellationTokenSource = new CancellationTokenSource();
                ReturnToIdle().Forget();
            }
        }

        private async UniTask DelayAttack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(attackCooldown));
            _canAttack = true;
        }

        private async UniTask ReturnToIdle()
        {
            _isOnAttack = true;

            await UniTask.Delay(TimeSpan.FromSeconds(attackToIdleTime), cancellationToken:
                _attackCancellationTokenSource.Token);

            if (_isDead) return;

            _canAttack   = true;
            _isOnAttack  = false;
            PlayerAttack = ePlayerAttack.NONE;
            ObjAnimator.SetTrigger(ANIM_PRAM_HASH_SET_IDLE);
            ObjAnimator.SetInteger(ANIM_PRAM_HASH_ATTACK_STATE, 0);

            DisposeAttackCancellationTokenSource();
        }

        private void DisposeAttackCancellationTokenSource()
        {
            if (_attackCancellationTokenSource == null) return;
            _attackCancellationTokenSource.Cancel();
            _attackCancellationTokenSource.Dispose();
            _attackCancellationTokenSource = null;
        }

        private bool _isDead = false;

        // TODO: 추상화
        public void OnDamaged(float damage)
        {
            CurrentHp -= damage;

            if (_currentHp <= 0 && !_isDead)
            {
                _isDead = true;
                ObjAnimator.SetTrigger(ANIM_PRAM_HASH_SET_DEAD);
            }
        }

        public void OnAttack()
        {
            weapon.OnAttack();
        }
#endregion Attack

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
            if (_playerManager.CurrentStamina < _staminaCost) return;
            if (_isOnGround) _desiredRoll = true;
        }

        protected virtual void OnAttack(bool value)
        {
            if (_isOnGround) _desiredAttack = true;
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