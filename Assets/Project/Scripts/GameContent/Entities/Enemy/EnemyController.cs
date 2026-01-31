using System;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.Entities.Enemy
{
    public abstract class EnemyController : CreatureObject, IDisposable
    {
        [field: SerializeField] [field: ReadOnly]
        private eMonsterState state;

        protected virtual Transform Target { get; set; }

        public virtual eMonsterState State
        {
            get => state;
            protected set
            {
                if (state == value) return;
                state = value;
            }
        }

        public override void Tick()
        {
            switch (State)
            {
                case eMonsterState.CREATED:
                    ProcessCreated();
                    break;
                case eMonsterState.IDLE:
                    ProcessIdle();
                    break;
                case eMonsterState.TRACING:
                    ProcessTracing();
                    break;
                case eMonsterState.ATTACK:
                    ProcessAttack();
                    break;
                case eMonsterState.DEAD:
                    ProcessDead();
                    break;
            }
        }

        public void Dispose()
        {
        }

        public virtual void OnDamaged(float damage)
        {
            if (State == eMonsterState.DEAD) return;
        }

#region ProcessState
        protected abstract void ProcessCreated();
        protected abstract void ProcessIdle();
        protected abstract void ProcessTracing();
        protected abstract void ProcessAttack();
        protected abstract void ProcessDead();
#endregion ProcessState
    }
}