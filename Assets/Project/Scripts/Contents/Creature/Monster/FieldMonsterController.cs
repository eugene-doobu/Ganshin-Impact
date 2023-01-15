using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    public class FieldMonsterController : MonsterController
    {
        [SerializeField] 
        private float rotationSmoothFactor = 8f;
        
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
        
        // TODO: A star 알고리즘을 이용한 이동
        protected override void Movement(float moveSpeed)
        {
            
        }

        #region ProcessState

        protected override void ProcessCreated()
        {
            
        }

        protected override void ProcessIdle()
        {
        }

        protected override void ProcessTracing()
        {
            if (ReferenceEquals(Target, null)) return;
            
            var direction = (Target.position - transform.position).normalized;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            Rigidbody.AddForce(direction * _moveSpeed);
            
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothFactor * Time.deltaTime);
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
