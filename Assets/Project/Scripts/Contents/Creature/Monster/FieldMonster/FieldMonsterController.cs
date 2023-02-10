#nullable enable

using UnityEngine;
using UnityEngine.AI;

namespace GanShin.Content.Creature.Monster
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent),typeof(CapsuleCollider))]
    public class FieldMonsterController : MonsterController
    {
        [SerializeField] private float _monsterSight        = 10f;
        [SerializeField] private float rotationSmoothFactor = 8f;

        private Animator        _animator        = null!;
        private NavMeshAgent    _navMeshAgent    = null!;
        private CapsuleCollider _capsuleCollider = null!;

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
            _capsuleCollider.radius    = _monsterSight;
        }
#endregion Initalize
        

        protected override void Movement(float moveSpeed)
        {
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
        }

        protected override void ProcessDead()
        {
        }
#endregion ProcessState
    }
}