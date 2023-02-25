#nullable enable

using System;
using System.ComponentModel;
using Cysharp.Threading.Tasks;
using System.Linq;
using GanShin.Data;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;
using UnityEngine.AI;

namespace GanShin.Content.Creature.Monster
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent),typeof(CapsuleCollider))]
    public class FieldMonsterController : MonsterController, IDataContextOwner
    {
        [SerializeField] private FieldMonsterTable _table;
        
        private readonly UIHpBarContext _uiHpBarContext = new UIHpBarContext();

        private Animator             _animator        = null!;
        private NavMeshAgent         _navMeshAgent    = null!;
        private CapsuleCollider      _capsuleCollider = null!;
        private FieldMonsterAnimBase _animController  = null!;
        private ContextHolder        _contextHolder   = null!;
        
        private float _attackTimer;
        private bool  _isAttacking;
        private bool  _isKnockBack;
        private bool  _isDead;

        private float _currentHp;

#region Properties
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
                    case eMonsterState.KNOCK_BACK:
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
                    case eMonsterState.KNOCK_BACK:
                        _navMeshAgent.isStopped = false;
                        _animController.OnDamaged();
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
                _currentHp = Mathf.Clamp(value, 0, _table.hp);

                _currentHp                = value;
                _uiHpBarContext.CurrentHp = (int)_currentHp;
            }
        }

        public INotifyPropertyChanged DataContext => _uiHpBarContext;
#endregion Properties

#region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            _animator        = GetComponent<Animator>();
            _navMeshAgent    = GetComponent<NavMeshAgent>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            switch (_table.monsterType)
            {
                case eFieldMonsterType.DEFAULT:
                    _animController = new FieldMonsterAnimatorController();
                    break;
                case eFieldMonsterType.HUMANOID:
                    _animController = new FieldHumanoidAnimatorController();
                    break;
            }
            _animController.Initialize(_animator);

            _contextHolder         = gameObject.AddComponent<ContextHolder>();
            _contextHolder.Context = _uiHpBarContext;
        }
        
        protected override void Start()
        {
            base.Start();
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Define.Tag.Player)) return;
            Target = other.transform;
            State  = eMonsterState.TRACING;
        }
#endregion MonoBehaviour

#region Initalize
        private void Initialize()
        {
            InitializeNavMeshAgent();
            InitializeCapsuleCollider();
        }

        private void InitializeNavMeshAgent()
        {
            _navMeshAgent.speed = _moveSpeed;
        }

        private void InitializeCapsuleCollider()
        {
            _capsuleCollider.center    = new Vector3(0, 0.5f, 0);
            _capsuleCollider.isTrigger = true;
            _capsuleCollider.radius    = _table.sight;
        }
#endregion Initalize
        

        protected override void Movement(float moveSpeed)
        {
            // 구조를 개선하며 제거 예정
        }

        public override void OnDamaged(float damage)
        {
            if (State == eMonsterState.DEAD) return;
            base.OnDamaged(damage);
            
            if (_currentHp <= 0)
            {
                State = eMonsterState.DEAD;
                return;
            }
            CurrentHp -= damage;
            
            GanDebugger.Log(nameof(FieldMonsterController),$"{gameObject.name} OnDamaged : {_currentHp}");
            
            State = eMonsterState.KNOCK_BACK;   
        }

#region ProcessState
        protected override void ProcessCreated()
        {
            Initialize();
            _uiHpBarContext.MaxHp      = (int)_table.hp;
            _uiHpBarContext.TargetName = name;
            CurrentHp                  = _table.hp;
            State                      = eMonsterState.IDLE;
        }

        protected override void ProcessIdle()
        {
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

        protected override void ProcessKnockBack()
        {
            if (_isKnockBack) return;
            _isKnockBack = true; 
            KnockBack().Forget();
        }

        protected override void ProcessAttack()
        {
            var targetPosition = Target.position;
            var distance       = Vector3.Distance(transform.position, targetPosition);

            if (TryChangeStateToIdle(distance)) return;
            if (TryChangeStateToTracing(distance)) return;

            if (_isAttacking) return;
            
            RotateToTarget(targetPosition);
            _attackTimer += Time.deltaTime;
            if (_attackTimer < _table.attackDelay) return;
            DoAttack().Forget();
        }

        // TODO: 고민. 풀링을 할 것인가 그냥 죽일것인가.
        protected override void ProcessDead()
        {
            if (_isDead) return;
            _isDead = true;
            DestroyOnDead().Forget();
        }

        private bool TryChangeStateToIdle(float distance)
        {
            if (_isAttacking) return false;
            if (!(distance > _table.traceRange)) return false;
            State = eMonsterState.IDLE;
            return true;
        }

        private bool TryChangeStateToAttack(float distance)
        {
            if (_isAttacking) return false;
            if (Target == null) return false;
            if (!(distance <= _table.attackRange)) return false;
            State = eMonsterState.ATTACK;
            return true;
        }
        
        private bool TryChangeStateToTracing(float distance)
        {
            if (_isAttacking) return false;
            if (Target == null) return false;
            if (!(distance > _table.attackRange)) return false;
            State = eMonsterState.TRACING;
            return true;
        }
#endregion ProcessState

#region Helper
        private void RotateToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, _table.rotationSmoothFactor * Time.deltaTime);
        }
        
        private async UniTask DoAttack()
        {
            if (Target == null) return;
            _attackTimer = 0f;
            
            // TODO: 투사체 공격도 처리
            _animController.OnAttack();
            _isAttacking = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_table.attackDuration));
            // TODO: SphereCast같은걸로 처리??, 연산 최적화 필요, 하드코딩 제거
            Physics.OverlapSphere(transform.position + transform.forward * 0.3f, 0.6f, Define.GetLayerMask(Define.eLayer.CHARACTER))
                   .Where(x => x.CompareTag(Define.Tag.Player))
                   .ToList()
                   .ForEach(x => x.GetComponent<PlayerController>().OnDamaged(_table.attackDamage));
            _isAttacking = false;
            _animController.OnIdle();
        }

        private Vector3 _knockBackPosition;
        
        private async UniTask KnockBack()
        {
            var tr = transform;
            _navMeshAgent.destination = tr.position - tr.forward * _table.knockBackPower;
            await UniTask.Delay(TimeSpan.FromSeconds(_table.knockDuration));
            
            if (_currentHp <= 0)
            {
                State = eMonsterState.DEAD;
                return;
            }
            
            if (Target == null)
            {
                State = eMonsterState.IDLE;
                return;
            }
            
            _navMeshAgent.destination = Target.position;
            _isKnockBack              = false;

            if (Target == null)
            {
                State = eMonsterState.IDLE;
                return;
            }
            
            var targetPosition = Target.position;
            var distance       = Vector3.Distance(tr.position, targetPosition);
            
            if (TryChangeStateToAttack(distance)) return;
            if (TryChangeStateToTracing(distance)) return;
            State = eMonsterState.IDLE;
        }
        
        private async UniTask DestroyOnDead()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_table.destroyDelay));
            Destroy(gameObject);
        }
#endregion Helper

#region Debug
#if UNITY_EDITOR
        [ContextMenu("TestDead")]
        public void TestDead()
        {
            State = eMonsterState.DEAD;
        }

        [ContextMenu("TestKnockBack")]
        public void TestKnockBack()
        {
            State = eMonsterState.KNOCK_BACK;
        }
#endif
#endregion Debug
    }
}