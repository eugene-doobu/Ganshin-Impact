using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.Content.Creature.Monster;
using GanShin.Content.Weapon;
using GanShin.Data;
using GanShin.GanObject;
using GanShin.InputSystem;
using GanShin.UI.Space;
using GanShin.UI;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.Content.Creature
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class PlayerController : CreatureObject
    {
#region Static

        protected static readonly int AnimPramHashIsMove      = Animator.StringToHash("IsMove");
        protected static readonly int AnimPramHashMoveSpeed   = Animator.StringToHash("MoveSpeed");
        protected static readonly int AnimPramHashRollStart   = Animator.StringToHash("RollStart");
        protected static readonly int AnimPramHashAttackState = Animator.StringToHash("AttackState");
        protected static readonly int AnimPramHashSetIdle     = Animator.StringToHash("SetIdle");
        protected static readonly int AnimPramHashSetDead     = Animator.StringToHash("SetDead");
        protected static readonly int AnimPramHashOnSkill     = Animator.StringToHash("OnSkill");
        protected static readonly int AnimPramHashOnSkill2    = Animator.StringToHash("OnSkill2");
        protected static readonly int AnimPramHashOnUltimate  = Animator.StringToHash("OnUltimate");

#endregion Static

#region Variables

        private CameraManager Camera        => ProjectManager.Instance.GetManager<CameraManager>();
        private PlayerManager PlayerManager => ProjectManager.Instance.GetManager<PlayerManager>();

        private PlayerAvatarContext _playerAvatarContext;

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

        [Space] [Header("Attack")] [SerializeField] [ReadOnly]
        private ePlayerAttack playerAttack;

        private bool _canAttack = true;
        private bool _desiredAttack;

        private bool _isOnGround;
        private bool _isDashOnLastFrame;

        private readonly float _rollStaminaCost         = 20f;
        private readonly float _dashStaminaCostOfSecond = 10f;

        protected CancellationTokenSource AttackCancellationTokenSource;

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
                playerAttack = value;
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

                _playerAvatarContext.CurrentHp = (int)_currentHp;
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

        protected bool IsOnSpecialAction { get; private set; }

        protected bool CanMove { get; set; } = true;

        protected CharacterController CC { get; private set; }

        protected PlayerManager Player => PlayerManager;

        private PlayerAvatarContext GetPlayerContext =>
            Player.GetAvatarContext(PlayerType);

        public bool IsDead { get; private set; }

        /// <summary>
        /// 스킬이나 연출 애니메이션 등 현재 애니메이션 상태에서 강제로 Idle 애니메이션으로 돌아가는 것을 막습니다.
        /// </summary>
        public bool IsCantToIdleAnimation { get; set; }

        protected Define.ePlayerAvatar PlayerType { get; set; }

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

        public override void Tick()
        {
            base.Tick();
            CheckOnGround();
            Movement();
            Roll();
            ApplyGravity();
            TryAttack();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            SolveFeetPositions();
            SetFeetIK();
        }

#endregion Mono

#region StateCheck

        private void InitializeAvatar()
        {
            CC  = GetComponent<CharacterController>();
            _tr = GetComponent<Transform>();

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
                _isOnGround = true;
            else
                _isOnGround = false;
        }

#endregion StateCheck

        private void AddInputEvent()
        {
            var input = ProjectManager.Instance.GetManager<InputSystemManager>();
            if (input == null)
                return;

            if (input.GetActionMap(eActionMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
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
            var input = ProjectManager.Instance.GetManager<InputSystemManager>();

            if (input?.GetActionMap(eActionMap.PLAYER_MOVEMENT) is not ActionMapPlayerMove actionMap)
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

        protected void ShowCutScene()
        {
            if (PlayerType == Define.ePlayerAvatar.NONE)
            {
                GanDebugger.ActorLogWarning("PlayerType is NONE");
                return;
            }

            var characterCutScene =
                ProjectManager.Instance.GetManager<UIManager>()?.GetGlobalUI(EGlobalUI.CHARACTER_CUT_SCENE) as
                    UIRootCharacterCutScene;
            if (characterCutScene != null)
                characterCutScene.OnCharacterCutScene(PlayerType);
        }

#region Movement

        private void Movement()
        {
            PlayMovementAnimation();
            if (_lastMovementValue == Vector2.zero) return;
            if (!CanMove) return;

            var mainCamera    = Camera.MainCamera;
            var cameraForward = Vector3.forward;
            var cameraRight   = Vector3.right;
            if (!ReferenceEquals(mainCamera, null))
            {
                cameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
                cameraRight   = Vector3.Cross(Vector3.up, cameraForward);
            }

            var moveSpeed = stat.moveSpeed;
            var dashCost  = _dashStaminaCostOfSecond * Time.deltaTime;
            if (IsOnSpecialAction && PlayerManager.CurrentStamina > dashCost)
            {
                moveSpeed          = stat.dashSpeed;
                _isDashOnLastFrame = true;

                PlayerManager.CurrentStamina -= dashCost;
            }
            else
            {
                _isDashOnLastFrame = false;
            }

            var direction = (cameraForward * _lastMovementValue.y + cameraRight * _lastMovementValue.x).normalized;
            CC.Move(direction * moveSpeed * Time.deltaTime);

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

            if (PlayerManager.CurrentStamina < _rollStaminaCost) return;
            PlayerManager.CurrentStamina -= _rollStaminaCost;

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
            CC.Move(Vector3.up * gravity * Time.deltaTime);
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

        public bool ApplyAttackDamage(Vector3 attackPosition, float attackRadius, float damage,
            Collider[] monsterColliders, Action<Collider> monsterCollider)
        {
            var len = Physics.OverlapSphereNonAlloc(attackPosition, attackRadius, monsterColliders,
                                                    Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; ++i)
            {
                var monster = monsterColliders[i].GetComponent<MonsterController>();
                if (ReferenceEquals(monster, null)) continue;

                monster.OnDamaged(damage);

                monsterCollider?.Invoke(monsterColliders[i]);
            }

            return len > 0;
        }

        public bool ApplyAttackDamageCapsule(Vector3 attackPosition, Vector3 attackDir, float attackRadius,
            float attackHeight, float damage,
            Collider[] monsterColliders, Action<Collider> monsterCollider)
        {
            var len = Physics.OverlapCapsuleNonAlloc(attackPosition + attackDir * (attackHeight * 0.5f - attackRadius),
                                                     attackPosition - attackDir * (attackHeight * 0.5f - attackRadius),
                                                     attackRadius, monsterColliders,
                                                     Define.GetLayerMask(Define.eLayer.MONSTER));
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

            if (AttackCancellationTokenSource is { IsCancellationRequested: false })
                await UniTask.Delay(TimeSpan.FromSeconds(attackToIdleTime), cancellationToken:
                                    AttackCancellationTokenSource.Token);

            if (IsCantToIdleAnimation || IsDead)
                return;

            CanMove     = true;
            _canAttack  = true;
            _isOnAttack = false;

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
            if (AttackCancellationTokenSource == null) return;
            AttackCancellationTokenSource.Cancel();
            AttackCancellationTokenSource.Dispose();
            AttackCancellationTokenSource = null;
        }

        public virtual void OnDamaged(float damage)
        {
            CurrentHp -= damage;

            CurrentUltimateGauge += stat.ultimateSkillChargeOnDamaged;

            if (_currentHp <= 0 && !IsDead)
            {
                IsDead  = true;
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
                value                                      += Time.deltaTime;
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
            IsOnSpecialAction = value;
            SpecialAction();
        }

        private void OnInteraction(bool value)
        {
        }

        private void OnRoll()
        {
            if (PlayerManager.CurrentStamina < _rollStaminaCost) return;
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

            CurrentUltimateGauge = 0f;
            UltimateSkill();
            RefreshUltimateGauge();
        }

#endregion ActionEvent

#region Animation

        private struct FootIkSolverData
        {
            public bool       IsDetectGround;
            public Vector3    FootPosition;
            public Quaternion FootRotation;
        }

        [Header("Feet Grounder")]
        public bool enableFeetIk = true;
        [Range(0, 2)] [SerializeField]
        private float heightFromGroundRaycast = 1.14f;
        [Range(0, 2)] [SerializeField]
        private float raycastDownDistance = 1.5f;
        [SerializeField]
        private LayerMask environmentLayer;
        [SerializeField]
        private float pelvisOffset;
        [Range(0, 1)] [SerializeField]
        private float pelvisUpAndDownSpeed = 0.28f;
        [Range(0, 1)] [SerializeField]
        private float feetToIkPositionSpeed = 0.5f;

#if UNITY_EDITOR
        [SerializeField]
        private bool showSolverDebug = true;
#endif // UNITY_EDITOR

        private float _lastPelvisPositionY, _lastRightFootPositionY, _lastLeftFootPositionY;

        private FootIkSolverData _rightFootSolverData, _leftFootSolverData;

        private void SolveFeetPositions()
        {
            if (!enableFeetIk) return;
            if (!HasAnimator) return;

            var rightFootPosition = AdjustFeetTarget(HumanBodyBones.RightFoot);
            var leftFootPosition = AdjustFeetTarget(HumanBodyBones.LeftFoot);

            // find and raycast to the ground to find positions
            _rightFootSolverData = FeetPositionSolver(rightFootPosition);
            _leftFootSolverData = FeetPositionSolver(leftFootPosition);
        }

        private void SetFeetIK()
        {
            if (!enableFeetIk) return;
            if (!HasAnimator) return;

            MovePelvisHeight();

            ObjAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            MoveFeetToIkPoint(AvatarIKGoal.RightFoot, _rightFootSolverData, ref _lastRightFootPositionY);

            ObjAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, _leftFootSolverData, ref _lastLeftFootPositionY);
        }

        private void MoveFeetToIkPoint(AvatarIKGoal foot, FootIkSolverData solverData, ref float lastFootPositionY)
        {
            var isSolved = solverData.IsDetectGround;
            var positionIkHolder = solverData.FootPosition;
            var rotationIkHolder = solverData.FootRotation;
            var targetIkPosition = ObjAnimator.GetIKPosition(foot);

            if (isSolved)
            {
                targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
                positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

                var yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPositionSpeed);
                targetIkPosition.y += yVariable;

                lastFootPositionY = yVariable;

                targetIkPosition = transform.TransformPoint(targetIkPosition);

                ObjAnimator.SetIKRotation(foot, rotationIkHolder);
            }

            ObjAnimator.SetIKPosition(foot, targetIkPosition);
        }

        private void MovePelvisHeight()
        {
            if (!_leftFootSolverData.IsDetectGround || !_rightFootSolverData.IsDetectGround || _lastPelvisPositionY == 0)
            {
                _lastPelvisPositionY = ObjAnimator.bodyPosition.y;
                return;
            }

            var transformPositionY = transform.position.y;
            var lOffsetPosition    = _leftFootSolverData.FootPosition.y - transformPositionY;
            var rOffsetPosition    = _rightFootSolverData.FootPosition.y - transformPositionY;

            var totalOffset = lOffsetPosition < rOffsetPosition ? lOffsetPosition : rOffsetPosition;
            var newPelvisPosition = ObjAnimator.bodyPosition + Vector3.up * totalOffset;

            newPelvisPosition.y = Mathf.Lerp(_lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

            ObjAnimator.bodyPosition = newPelvisPosition;
            _lastPelvisPositionY = ObjAnimator.bodyPosition.y;
        }

        private FootIkSolverData FeetPositionSolver(Vector3 fromSkyPosition)
        {
#if UNITY_EDITOR
            if (showSolverDebug)
            {
                Debug.DrawLine(fromSkyPosition,
                               fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast),
                               Color.yellow);
            }
#endif

            if (!Physics.Raycast(fromSkyPosition, Vector3.down, out var feetOutHit,
                                 raycastDownDistance + heightFromGroundRaycast, environmentLayer))
            {
                return new FootIkSolverData
                {
                    IsDetectGround = false,
                    FootPosition   = Vector3.zero,
                    FootRotation   = Quaternion.identity
                };
            }

            var feetIkPositions = fromSkyPosition;
            feetIkPositions.y = feetOutHit.point.y + pelvisOffset;
            var feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

            return new FootIkSolverData
            {
                IsDetectGround = true,
                FootPosition   = feetIkPositions,
                FootRotation   = feetIkRotations
            };

        }

        private Vector3 AdjustFeetTarget(HumanBodyBones foot)
        {
            var feetPositions = ObjAnimator.GetBoneTransform(foot).position;
            feetPositions.y = transform.position.y + heightFromGroundRaycast;
            return feetPositions;
        }

#endregion Animation
    }
}