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
        protected Transform Target;

        [field: SerializeField, ReadOnly] public eMonsterState State { get; protected set; } = eMonsterState.CREATED;

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