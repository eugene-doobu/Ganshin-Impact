#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.Data;
using GanShin.Space.UI;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;
using UnityEngine.AI;

namespace GanShin.Content.Creature.Monster
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent), typeof(CapsuleCollider))]
    public class FieldMonsterController : MonsterController, IDataContextOwner
    {
        protected static readonly Collider[] CharacterCollider = new Collider[2];

        [SerializeField] private FieldMonsterTable table = null!;

        [SerializeField] private float attackForwardDistance = 0.1f;
        [SerializeField] private float attackRadius          = 1.2f;

        private FieldMonsterContext? _fieldMonsterContext;

        private Animator             _animator       = null!;
        private FieldMonsterAnimBase _animController = null!;

        private float           _attackTimer;
        private CapsuleCollider _capsuleCollider = null!;

        private float        _currentHp;
        private bool         _isAttacking;
        private bool         _isDead;
        private NavMeshAgent _navMeshAgent = null!;

        private Transform? _playerTarget;
        
        public FieldMonsterTable Table => table;

#region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            _animator        = GetComponent<Animator>();
            _navMeshAgent    = GetComponent<NavMeshAgent>();
            _capsuleCollider = GetComponent<CapsuleCollider>();

            switch (table.monsterType)
            {
                case eFieldMonsterType.DEFAULT:
                    _animController = new FieldMonsterAnimatorController();
                    break;
                case eFieldMonsterType.HUMANOID:
                    _animController = new FieldHumanoidAnimatorController();
                    break;
            }

            _animController.Initialize(_animator);
        }
#endregion MonoBehaviour
        
        protected override void Initialize()
        {
            base.Initialize();
            var managerContext = Util.GetContext<FieldMonsterManagerContext>();
            if (managerContext == null) return;
            
            InitializeNavMeshAgent();
            InitializeCapsuleCollider();
            
            if (managerContext.TryGet(Id, out var context) && context is FieldMonsterContext fieldMonsterContext)
            {
                fieldMonsterContext.MaxHp      = (int)table.hp;
                fieldMonsterContext.TargetName = name;
                _fieldMonsterContext           = fieldMonsterContext;
            }
            
            CurrentHp = table.hp;
            ObjectHeight = table.monsterHeight;
        }

        public override void Tick()
        {
            base.Tick();
            if (_fieldMonsterContext == null) return;
            _fieldMonsterContext.Position = transform.position + Vector3.up * ObjectHeight;
        }

        public override void OnDamaged(float damage)
        {
            if (State == eMonsterState.DEAD) return;
            base.OnDamaged(damage);

            CurrentHp -= damage;
            GanDebugger.Log(nameof(FieldMonsterController), $"{gameObject.name} OnDamaged : {_currentHp}");

            if (_currentHp <= 0)
            {
                State = eMonsterState.DEAD;
                return;
            }

            _animController.OnDamaged();

            if (Target == null)
            {
                State = eMonsterState.IDLE;
                return;
            }

            var targetPosition = Target.position;
            var distance       = Vector3.Distance(transform.position, targetPosition);

            if (TryChangeStateToAttack(distance)) return;
            if (TryChangeStateToTracing(distance)) return;
            State = eMonsterState.IDLE;
        }

        private void OnPlayerChanged(PlayerController player)
        {
            Target = player.transform;
        }

#region Debug
#if UNITY_EDITOR
        [ContextMenu("TestDead")]
        public void TestDead()
        {
            State = eMonsterState.DEAD;
        }
#endif // UNITY_EDITOR
#endregion Debug

#region Properties
        protected override Transform? Target
        {
            get => _playerTarget;
            set
            {
                _playerTarget = value;
                
                var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
                if (playerManager == null) return;
                
                if (value != null) playerManager.OnPlayerChanged += OnPlayerChanged;
                else playerManager.OnPlayerChanged               -= OnPlayerChanged;
            }
        }

        public override eMonsterState State
        {
            get => base.State;
            protected set
            {
                var prevValue = base.State;

                if (base.State == value) return;
                base.State = value;

                switch (prevValue)
                {
                    case eMonsterState.IDLE:
                        break;
                    case eMonsterState.TRACING:
                        _navMeshAgent.isStopped = true;
                        break;
                    case eMonsterState.ATTACK:
                        _attackTimer = 0f;
                        _isAttacking = false;
                        break;
                }

                switch (value)
                {
                    case eMonsterState.IDLE:
                        _animController.OnIdle();
                        Target = null;
                        break;
                    case eMonsterState.TRACING:
                        _navMeshAgent.isStopped = false;
                        _animController.OnMove();
                        break;
                    case eMonsterState.ATTACK:
                        _animController.OnIdle();
                        break;
                    case eMonsterState.DEAD:
                        _animController.OnDie();
                        break;
                }
            }
        }

        private float CurrentHp
        {
            get => _currentHp;
            set
            {
                if (Mathf.Approximately(_currentHp, value)) return;
                _currentHp = Mathf.Clamp(value, 0, table.hp);

                _currentHp = value;
                if (_fieldMonsterContext != null)
                    _fieldMonsterContext.CurrentHp = (int)_currentHp;
            }
        }

        public Context? DataContext => _fieldMonsterContext;
#endregion Properties

#region Initalize
        private void InitializeNavMeshAgent()
        {
            _navMeshAgent.speed = table.moveSpeed;
        }

        private void InitializeCapsuleCollider()
        {
            _capsuleCollider.center    = new Vector3(0, 0.5f, 0);
            _capsuleCollider.isTrigger = true;
        }
#endregion Initalize

#region ProcessState
        protected override void ProcessCreated()
        {
            CurrentHp = table.hp;
            State     = eMonsterState.IDLE;
        }

        protected override void ProcessIdle()
        {
            var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
            var playerTr      = playerManager?.CurrentPlayerTransform;
            if (playerTr == null) return;

            var diff = playerTr.position - transform.position;
            if (diff.magnitude > table.sight) return;

            Target = playerTr;
            State  = eMonsterState.TRACING;
        }

        protected override void ProcessTracing()
        {
            if (ReferenceEquals(Target, null)) return;

            var targetPosition = Target.position;
            var distance       = Vector3.Distance(transform.position, targetPosition);

            if (TryChangeStateToAttack(distance)) return;
            if (TryChangeStateToIdle(distance)) return;

            _navMeshAgent.destination = targetPosition;
            RotateToTarget(targetPosition);
        }

        protected override void ProcessAttack()
        {
            if (Target == null)
            {
                State = eMonsterState.IDLE;
                return;
            }

            var targetPosition = Target.position;
            var distance       = Vector3.Distance(transform.position, targetPosition);

            if (TryChangeStateToIdle(distance)) return;
            if (TryChangeStateToTracing(distance)) return;

            if (_isAttacking) return;

            RotateToTarget(targetPosition);
            _attackTimer += Time.deltaTime;
            if (_attackTimer < table.attackDelay) return;
            DoAttack().Forget();
        }

        // TODO: 고민. 풀링을 할 것인가 그냥 죽일것인가.
        protected override void ProcessDead()
        {
            if (_isDead) return;
            _isDead = true;
            
            _capsuleCollider.enabled = false;

            var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
            if (playerManager?.CurrentPlayer == null) return;

            playerManager.CurrentPlayer.CurrentUltimateGauge
                += playerManager.CurrentPlayer.Stat.ultimateSkillChargeOnKill;

            DestroyOnDead().Forget();
        }

        private bool TryChangeStateToIdle(float distance)
        {
            if (_isAttacking) return false;
            if (!(distance > table.traceRange)) return false;
            State = eMonsterState.IDLE;
            return true;
        }

        private bool TryChangeStateToAttack(float distance)
        {
            if (_isAttacking) return false;
            if (Target == null) return false;
            if (!(distance <= table.attackRange)) return false;
            State = eMonsterState.ATTACK;
            return true;
        }

        private bool TryChangeStateToTracing(float distance)
        {
            if (_isAttacking) return false;
            if (Target == null) return false;
            if (!(distance > table.attackRange)) return false;
            State = eMonsterState.TRACING;
            return true;
        }
#endregion ProcessState

#region Helper
        private void RotateToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);

            if (direction == Vector3.zero) return;

            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, table.rotationSmoothFactor * Time.deltaTime);
        }

        private async UniTask DoAttack()
        {
            if (Target == null) return;
            _attackTimer = 0f;

            // TODO: 투사체 공격도 처리
            _animController.OnAttack();
            _isAttacking = true;
            await UniTask.Delay(TimeSpan.FromSeconds(table.attackDuration), cancellationToken: destroyCancellationToken);
            var tr       = transform;
            var position = tr.position + tr.forward * attackForwardDistance;
            var len = Physics.OverlapSphereNonAlloc(position, attackRadius, CharacterCollider,
                                                    Define.GetLayerMask(Define.eLayer.CHARACTER));
            for (var i = 0; i < len; ++i)
            {
                var player = CharacterCollider[i].GetComponent<PlayerController>();
                if (ReferenceEquals(player, null)) continue;
                if (!player.CompareTag(Define.Tag.Player)) continue;

                player.OnDamaged(table.attackDamage);
            }

            _isAttacking = false;
            _animController.OnIdle();
        }

        private async UniTask DestroyOnDead()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(table.destroyDelay), cancellationToken: destroyCancellationToken);
            Destroy(gameObject);
        }
#endregion Helper
    }
}