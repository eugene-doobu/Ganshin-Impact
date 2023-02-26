using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class RikoController : PlayerController, IAttackAnimation
    {
        protected override void Attack()
        {
            bool isTryAttack = false;
            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                    if (!isOnUltimate)
                    {
                        PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK1;
                        ObjAnimator.SetInteger(AnimPramHashAttackState, 1);
                    }
                    else
                    {
                        PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK1;
                        ObjAnimator.SetInteger(AnimPramHashAttackState, 5);
                    }

                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK1:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 2);
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK2:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 3);
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTAK3:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTAK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 4);
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK1:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 6);
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK2:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 7);
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTI_ATTAK3:
                    PlayerAttack = ePlayerAttack.RIKO_ULTI_ATTAK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 8);
                    isTryAttack = true;
                    break;
            }

            if (isTryAttack)
            {
                if (_attackCancellationTokenSource != null)
                    DisposeAttackCancellationTokenSource();
                _attackCancellationTokenSource = new CancellationTokenSource();
                ReturnToIdle().Forget();
            }
        }

        protected override void Skill()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);
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
