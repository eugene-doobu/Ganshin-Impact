using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    public abstract class MonsterController : CreatureController, IDisposable
    {
        protected virtual Transform Target { get; set; }

        [field: SerializeField, ReadOnly] private eMonsterState state;

        public virtual eMonsterState State
        {
            get => state;
            protected set
            {
                if (state == value) return;
                state = value;
            }
        }

        public virtual void OnDamaged(float damage, float additionalKnockBack = 0)
        {
            if (State == eMonsterState.DEAD) return;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected virtual void OnDestroy()
        {
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
                case eMonsterState.KNOCK_BACK:
                    ProcessKnockBack();
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

        public void SetCaught()
        {
            State = eMonsterState.CAUGHT;
        }
#region ProcessState

        protected abstract void ProcessCreated();
        protected abstract void ProcessIdle();
        protected abstract void ProcessTracing();
        protected abstract void ProcessKnockBack();
        protected abstract void ProcessAttack();
        protected abstract void ProcessDead();

#endregion ProcessState
    }
}