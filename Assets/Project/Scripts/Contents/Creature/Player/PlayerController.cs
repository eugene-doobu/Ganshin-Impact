using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.Content.Creature.Monster;
using GanShin.InputSystem;
using GanShin.Utils;
using GanShin.Content.Weapon;
using GanShin.Data;
using GanShin.UI;
using GanShin.Space.UI;
using UnityEngine;
using Zenject;

namespace GanShin.Content.Creature
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class PlayerController : CreatureController
    {
#region Static
        protected static readonly int AnimPramHashIsMove      = Animator.StringToHash("IsMove");
        protected static readonly int AnimPramHashMoveSpeed   = Animator.StringToHash("MoveSpeed");
        protected static readonly int AnimPramHashRollStart   = Animator.StringToHash("RollStart");
        protected static readonly int AnimPramHashAttackState = Animator.StringToHash("AttackState");
        protected static readonly int AnimPramHashSetIdle     = Animator.StringToHash("SetIdle");
        protected static readonly int AnimPramHashSetDead     = Animator.StringToHash("SetDead");
        protected static readonly int AnimPramHashOnSkill     = Animator.StringToHash("OnSkill");
        protected static readonly int AnimPramHashOnSkill2     = Animator.StringToHash("OnSkill2");
        protected static readonly int AnimPramHashOnUltimate  = Animator.StringToHash("OnUltimate");
#endregion Static

#region Variables
        [Inject] protected UIRootCharacterCutScene CharacterCutScene;
        
        [Inject] private InputSystemManager _input;
        [Inject] private CameraManager      _camera;
        [Inject] private PlayerManager      _playerManager;

        private CharacterController _characterController;
        private PlayerAvatarContext      _playerAvatarContext;

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
        private bool _isOnSpecialAction;
        
        [Space] [Header("GroundCheck")] [SerializeField]
        private float rayStartPosOffset = 0.3f;

        [SerializeField] private float     groundCheckDistance = 0.5f;
        [SerializeField] private float     groundCheckRadius   = 0.1f;
        [SerializeField] private LayerMask groundLayerMask;

        [Space]
        [Header("Attack")]
        [SerializeField] [ReadOnly] private ePlayerAttack playerAttack;
        
        private bool _canAttack = true;
        private bool _desiredAttack;

        private bool _isOnGround;
        private bool _isDashOnLastFrame;

        private float _rollStaminaCost         = 20f;
        private float _dashStaminaCostOfSecond = 10f;

        private bool _isDead = false;

        protected CancellationTokenSource _attackCancellationTokenSource;
        
        private bool  _isOnAttack;
        private float _currentHp;
        
        private float _currentUltimateGauge;
        private bool  _isAvailableSkill  = true;
        private bool  _isAvailableSkill2 = true;
#endregion Variables

#region Properties
        public CharacterStatTable Stat => stat;

        public ePlayerAttack PlayerAttack
        {
            get => playerAttack;

            protected set
            {
                if (playerAttack == value) return;
                playerAttack      = value;
                if (weapon != null)
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

                _playerAvatarContext.CurrentHp = (int) _currentHp;
            }
        }
        
        public float CurrentUltimateGauge
        {
            get => _currentUltimateGauge;
            set
            {
                if (Mathf.Approximately(_currentUltimateGauge, value)) return;
                _currentUltimateGauge = Mathf.Clamp(value, 0, stat.ultimateSkillAvailabilityGauge);
                RefreshUltimateGauge();
            }
        }
        
        protected PlayerWeaponBase Weapon => weapon;
        
        protected bool IsOnSpecialAction => _isOnSpecialAction;

        protected bool CanMove { get; set; } = true;

        protected CharacterController CC => _characterController;
        
        protected PlayerManager Player => _playerManager;

        public abstract PlayerAvatarContext GetPlayerContext { get; }
        
        public bool IsDead => _isDead;
#endregion Properties

#region Mono
        protected override void Awake()
        {
            base.Awake();

            InitializeAvatar();
            InitializeWeapon();

            _playerAvatarContext = GetPlayerContext;
            
            _playerAvatarContext.MaxHp = (int)stat.hp;
            CurrentHp                  = stat.hp;
        }

        private void OnEnable()
        {
            AddInputEvent();
            RefreshUltimateGauge();
        }

        private void OnDisable()
        {
            RemoveInputEvent();
        }

        protected override void Update()
        {
            CheckOnGround();
            Movement();
            Roll();
            ApplyGravity();
            TryAttack();
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
            if (weapon == null) return;
            weapon.Owner = this;
        }

        private void CheckOnGround()
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

        private void AddInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
            {
                GanDebugger.LogError(nameof(PlayerController), "actionMap is null!");
                return;
            }

            actionMap.OnAttack        += OnAttack;
            actionMap.OnSpecialAction += OnSpecialAction;
            actionMap.OnInteraction   += OnInteraction;
            actionMap.OnRoll          += OnRoll;
            actionMap.OnMovement      += OnMovement;
            actionMap.OnBaseSkill     += OnBaseSkill;
            actionMap.OnBaseSkill2    += OnBaseSkill2;
            actionMap.OnUltimateSkill += OnUltimateSkill;
        }

        private void RemoveInputEvent()
        {
            if (_input.GetActionMap(eActiomMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
                return;

            actionMap.OnAttack        -= OnAttack;
            actionMap.OnSpecialAction -= OnSpecialAction;
            actionMap.OnInteraction   -= OnInteraction;
            actionMap.OnRoll          -= OnRoll;
            actionMap.OnMovement      -= OnMovement;
            actionMap.OnBaseSkill     -= OnBaseSkill;
            actionMap.OnBaseSkill2    -= OnBaseSkill2;
            actionMap.OnUltimateSkill -= OnUltimateSkill;
        }

#region Movement
        private void Movement()
        {
            PlayMovementAnimation();
            if (_lastMovementValue == Vector2.zero) return;
            if (!CanMove) return;

            var mainCamera    = _camera.MainCamera;
            var cameraForward = Vector3.forward;
            var cameraRight   = Vector3.right;
            if (!ReferenceEquals(mainCamera, null))
            {
                cameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
                cameraRight   = Vector3.Cross(Vector3.up, cameraForward);
            }

            var moveSpeed = stat.moveSpeed;
            var dashCost  = _dashStaminaCostOfSecond * Time.deltaTime;
            if (_isOnSpecialAction && _playerManager.CurrentStamina > dashCost)
            {
                moveSpeed          = stat.dashSpeed;
                _isDashOnLastFrame = true;
                
                _playerManager.CurrentStamina -= dashCost;
            }
            else
            {
                _isDashOnLastFrame = false;
            }
            
            var direction = (cameraForward * _lastMovementValue.y + cameraRight * _lastMovementValue.x).normalized;
            _characterController.Move(direction * moveSpeed * Time.deltaTime);

            var targetRotation = Quaternion.LookRotation(direction);
            _tr.rotation = Quaternion.Slerp(_tr.rotation, targetRotation, rotationSmoothFactor * Time.deltaTime);
        }

        private void PlayMovementAnimation()
        {
            if (!HasAnimator) return;

            var speed = _isDashOnLastFrame ? 1.5f : 1f;
            speed = CanMove ? speed : 0f;
            _moveAnimValue = Mathf.Lerp(_moveAnimValue, _lastMovementValue.magnitude * speed,
                rotationSmoothFactor * Time.deltaTime);

            ObjAnimator.SetBool(AnimPramHashIsMove, _lastMovementValue != Vector2.zero);
            ObjAnimator.SetFloat(AnimPramHashMoveSpeed, _moveAnimValue);
        }

        private void Roll()
        {
            if (!_desiredRoll) return;
            _desiredRoll = false;

            if (!_canRoll) return;
            
            if (_playerManager.CurrentStamina < _rollStaminaCost) return;
            _playerManager.CurrentStamina -= _rollStaminaCost;
            
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
            ObjAnimator.SetTrigger(AnimPramHashRollStart);
        }

        private void ApplyGravity()
        {
            _characterController.Move(Vector3.up * gravity * Time.deltaTime);
        }
#endregion Movement

#region Attack
        private void TryAttack()
        {
            if (!_desiredAttack) return;
            _desiredAttack = false;

            if (!_canAttack) return;
            _canAttack = false;
            DelayAttack().Forget();

            Attack();
        }

        protected abstract void Attack();

        protected abstract void Skill();

        protected abstract void Skill2();
        
        protected abstract void UltimateSkill();

        protected abstract void SpecialAction();
        
        public bool ApplyAttackDamage(Vector3 attackPosition, float attackRadius, float damage, Collider[] monsterColliders, Action<Collider> monsterCollider)
        {
            var len = Physics.OverlapSphereNonAlloc(attackPosition, attackRadius, monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; ++i)
            {
                var monster = monsterColliders[i].GetComponent<MonsterController>();
                if (ReferenceEquals(monster, null)) continue;
                
                monster.OnDamaged(damage);
                
                monsterCollider?.Invoke(monsterColliders[i]);
            }

            return len > 0;
        }

        protected async UniTask DelayAttack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stat.attackCoolTime));
            _canAttack = true;
        }

        protected async UniTask ReturnToIdle(float attackToIdleTime = 0.5f, bool isAttackStateClearNow = true)
        {
            _isOnAttack = true;

            await UniTask.Delay(TimeSpan.FromSeconds(attackToIdleTime), cancellationToken:
                _attackCancellationTokenSource.Token);

            if (_isDead) return;

            CanMove      = true;
            _canAttack   = true;
            _isOnAttack  = false;
            
            if (isAttackStateClearNow)
                PlayerAttack = ePlayerAttack.NONE;
            else
                AttackStateClearAsync(PlayerAttack).Forget();
            
            ObjAnimator.SetTrigger(AnimPramHashSetIdle);
            ObjAnimator.SetInteger(AnimPramHashAttackState, 0);

            DisposeAttackCancellationTokenSource();
        }

        protected async UniTask AttackStateClearAsync(ePlayerAttack previousState)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stat.previousAttackStateHoldTime), cancellationToken:
                gameObject.GetCancellationTokenOnDestroy());
            if (previousState == PlayerAttack)
                PlayerAttack = ePlayerAttack.NONE;
        }

        protected void DisposeAttackCancellationTokenSource()
        {
            if (_attackCancellationTokenSource == null) return;
            _attackCancellationTokenSource.Cancel();
            _attackCancellationTokenSource.Dispose();
            _attackCancellationTokenSource = null;
        }

        public virtual void OnDamaged(float damage)
        {
            CurrentHp -= damage;

            CurrentUltimateGauge += stat.ultimateSkillChargeOnDamaged;

            if (_currentHp <= 0 && !_isDead)
            {
                _isDead  = true;
                CanMove = false;
                ObjAnimator.SetTrigger(AnimPramHashSetDead);
                _playerAvatarContext.IsDead = true;
                RemoveInputEvent();
            }
        }
        
        public void OnHealed(float heal)
        {
            CurrentHp += heal;
        }
        
        private async UniTask SkillCoolTime()
        {
            _isAvailableSkill = false;
            float value = 0, coolTime = stat.baseSkillCoolTime;
            while (value < coolTime)
            {
                GetPlayerContext.BaseSkillCoolTimePercent =  1 - value / coolTime;
                value                                     += Time.deltaTime;
                await UniTask.Yield();
            }

            GetPlayerContext.BaseSkillCoolTimePercent = 0f;
            _isAvailableSkill                         = true;
        }
        
        private async UniTask Skill2CoolTime()
        {
            _isAvailableSkill2 = false;
            float value = 0, coolTime = stat.baseSkill2CoolTime;
            while (value < coolTime)
            {
                GetPlayerContext.BaseSkill2CoolTimePercent =  1 - value / coolTime;
                value                                     += Time.deltaTime;
                await UniTask.Yield();
            }

            GetPlayerContext.BaseSkill2CoolTimePercent = 0f;
            _isAvailableSkill2                         = true;
        }

        private void RefreshUltimateGauge()
        {
            GetPlayerContext.UltimateGaugePercent = 1 - _currentUltimateGauge / stat.ultimateSkillAvailabilityGauge;
        }
#endregion Attack

#region ActionEvent
        private void OnMovement(Vector2 value)
        {
            _lastMovementValue = value;
        }

        private void OnSpecialAction(bool value)
        {
            _isOnSpecialAction = value;
            SpecialAction();
        }

        private void OnInteraction(bool value)
        {
        }

        private void OnRoll()
        {
            if (_playerManager.CurrentStamina < _rollStaminaCost) return;
            if (_isOnGround) _desiredRoll = true;
        }

        protected virtual void OnAttack(bool value)
        {
            if (_isOnGround) _desiredAttack = true;
        }

        protected virtual void OnBaseSkill(bool value)
        {
            if (!_isAvailableSkill)
            {
                GanDebugger.Log(nameof(PlayerController), "스킬 쿨타임입니다.");
                return;
            }

            SkillCoolTime().Forget();
            CurrentUltimateGauge += stat.ultimateSkillChargeOnSkill;
            Skill();
        }

        protected virtual void OnBaseSkill2(bool value)
        {
            if (!_isAvailableSkill2)
            {
                GanDebugger.Log(nameof(PlayerController), "스킬 쿨타임입니다.");
                return;
            }

            Skill2CoolTime().Forget();
            CurrentUltimateGauge += stat.ultimateSkillChargeOnSkill2;
            Skill2();
        }

        protected virtual void OnUltimateSkill(bool value)
        {
            if (stat.ultimateSkillAvailabilityGauge > _currentUltimateGauge)
            {
                GanDebugger.Log(nameof(PlayerController), "궁극기 게이지가 부족합니다.");
                return;
            }
            
            _currentUltimateGauge = 0f;
            UltimateSkill();
            RefreshUltimateGauge();
        }
#endregion ActionEvent
    }
}