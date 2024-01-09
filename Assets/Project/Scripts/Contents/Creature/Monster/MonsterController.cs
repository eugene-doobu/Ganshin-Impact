using System;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    public abstract class MonsterController : CreatureController, IDisposable
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

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
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

        protected virtual void OnDestroy()
        {
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