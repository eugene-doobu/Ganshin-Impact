#nullable enable

using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GanShin.Data;
using UnityEngine;
using UnityEngine.AI;

namespace GanShin.Content.Creature.Monster
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent),typeof(CapsuleCollider))]
    public class FieldMonsterController : MonsterController
    {
        [SerializeField] private FieldMonsterTable _table;

        private Animator             _animator        = null!;
        private NavMeshAgent         _navMeshAgent    = null!;
        private CapsuleCollider      _capsuleCollider = null!;
        private FieldMonsterAnimBase _animController  = null!;
        
        private float _attackTimer;
        private bool  _isAttacking;
        private bool  _isKnockBack;
        private bool  _isDead;

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

#region ProcessState
        protected override void ProcessCreated()
        {
            Initialize();
            State = eMonsterState.IDLE;
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
            _isAttacking = false;
            _animController.OnIdle();
        }
        
        private async UniTask KnockBack()
        {
            var targetPosition = Target.position;
            var distance       = Vector3.Distance(transform.position, targetPosition);
            
            transform.DOMove(transform.forward * -_table.knockBackPower, _table.knockDuration).SetEase(_table.knockBackEase);
            await UniTask.Delay(TimeSpan.FromSeconds(_table.knockDuration));
            _isKnockBack = false;
            
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