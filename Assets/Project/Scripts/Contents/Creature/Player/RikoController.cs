using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class RikoController : PlayerController, IAttackAnimation
    {
        private RikoStatTable _rikoStat;
        
        protected override void Awake()
        {
            base.Awake();
            
            _rikoStat = Stat as RikoStatTable;
            if (_rikoStat == null)
            {
                GanDebugger.LogError("Stat asset is not RikoStatTable");
                return;
            }
        }
        
        protected override void Attack()
        {
            bool  isTryAttack  = false;
            float attackDelay  = 1f;
            bool  isLastAttack = false;
            
            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                    if (!isOnUltimate)
                    {
                        PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK1;
                        ObjAnimator.SetInteger(AnimPramHashAttackState, 1);
                        attackDelay = _rikoStat.attack1Delay;
                    }
                    else
                    {
                        PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK1;
                        ObjAnimator.SetInteger(AnimPramHashAttackState, 5);
                        attackDelay = _rikoStat.attack1Delay;
                    }

                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK1:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 2);
                    attackDelay = _rikoStat.attack2Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK2:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 3);
                    attackDelay = _rikoStat.attack3Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK3:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 4);
                    attackDelay  = _rikoStat.attack4Delay;
                    isTryAttack  = true;
                    isLastAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK1:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 6);
                    attackDelay = _rikoStat.attack2Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK2:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 7);
                    attackDelay = _rikoStat.attack3Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK3:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 8);
                    attackDelay  = _rikoStat.attack4Delay;
                    isTryAttack  = true;
                    isLastAttack = true;
                    break;
            }

            if (isTryAttack)
            {
                CanMove = false;
                if (_attackCancellationTokenSource != null)
                    DisposeAttackCancellationTokenSource();
                _attackCancellationTokenSource = new CancellationTokenSource();
                ReturnToIdle(attackDelay, isLastAttack).Forget();
            }
        }

        protected override void Skill()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);
            
            CanMove = false;
            if (_attackCancellationTokenSource != null)
                DisposeAttackCancellationTokenSource();
            _attackCancellationTokenSource = new CancellationTokenSource();
            ReturnToIdle(_rikoStat.skillDelay).Forget();
        }

        protected override void UltimateSkill()
        {
            
        }

#region AnimEvents
        [UsedImplicitly]
        public void OnAnimAttack()
        {
            Weapon.OnAttack();
        }
#endregion AnimEvents
    }
}
