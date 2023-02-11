#nullable enable

using System;
using UnityEngine;
using UnityEngine.AI;

namespace GanShin.Content.Creature.Monster
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent),typeof(CapsuleCollider))]
    public class FieldMonsterController : MonsterController
    {
        [SerializeField] private float monsterSight         = 8f;
        [SerializeField] private float rotationSmoothFactor = 8f;
        
        [SerializeField] protected float attackRange = 2f;
        [SerializeField] protected float attackDelay = 1f;
        [SerializeField] protected float attackTimer = 0f;
        [SerializeField] protected bool  isAttacking = false;
        [SerializeField] protected float traceRange  = 12f;

        private Animator        _animator        = null!;
        private NavMeshAgent    _navMeshAgent    = null!;
        private CapsuleCollider _capsuleCollider = null!;

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
                    case eMonsterState.CREATED:
                        break;
                    case eMonsterState.IDLE:
                        break;
                    case eMonsterState.TRACING:
                        _navMeshAgent.isStopped = true;
                        break;
                    case eMonsterState.KNOCK_BACK:
                        break;
                    case eMonsterState.ATTACK:
                        attackTimer = 0f;
                        break;
                    case eMonsterState.DEAD:
                        break;
                }
                
                switch (value)
                {
                    case eMonsterState.CREATED:
                        break;
                    case eMonsterState.IDLE:
                        Target = null;
                        break;
                    case eMonsterState.TRACING:
                        _navMeshAgent.isStopped = false;
                        break;
                    case eMonsterState.KNOCK_BACK:
                        break;
                    case eMonsterState.ATTACK:
                        break;
                    case eMonsterState.DEAD:
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
            _capsuleCollider.radius    = monsterSight;
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

            var direction = (targetPosition - transform.position).normalized;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);

            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothFactor * Time.deltaTime);
        }

        protected override void ProcessKnockBack()
        {
        }

        protected override void ProcessAttack()
        {
            var targetPosition = Target.position;
            var distance       = Vector3.Distance(transform.position, targetPosition);

            if (TryChangeStateToIdle(distance)) return;
            if (TryChangeStateToTracing(distance)) return;
            
            // attack!!
        }

        protected override void ProcessDead()
        {
        }

        private bool TryChangeStateToIdle(float distance)
        {
            if (!(distance > traceRange)) return false;
            State = eMonsterState.IDLE;
            return true;
        }

        private bool TryChangeStateToAttack(float distance)
        {
            if (Target == null) return false;
            if (!(distance <= attackRange)) return false;
            State = eMonsterState.ATTACK;
            return true;
        }
        
        private bool TryChangeStateToTracing(float distance)
        {
            if (Target == null) return false;
            if (!(distance > attackRange)) return false;
            State = eMonsterState.TRACING;
            return true;
        }
#endregion ProcessState
    }
}