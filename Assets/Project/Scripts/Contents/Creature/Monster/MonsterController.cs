using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.Content.Creature.Monster
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class MonsterController : CreatureController, IDisposable
    {
        protected Rigidbody Rigidbody;
        protected Transform Target;

        private CancellationTokenSource _cancellationTokenSource; 

        [field:SerializeField, ReadOnly]
        public eMonsterState State { get; protected set; } = eMonsterState.CREATED;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeRigidBody();
        }

        protected override void Start()
        {
            base.Start();
            
            _cancellationTokenSource = new CancellationTokenSource();
            StateMachine().Forget();
        }

        protected virtual void OnDestroy()
        {
            DisposeCancellationToken();
        }

        public void DisposeCancellationToken()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTask StateMachine()
        {
            if (_cancellationTokenSource == null) return;
            while (true)
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
                await UniTask.Yield(_cancellationTokenSource.Token);
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

        private void InitializeRigidBody()
        {
            Rigidbody             = GetComponent<Rigidbody>();
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
