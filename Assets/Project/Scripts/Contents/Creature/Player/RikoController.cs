using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Data;
using GanShin.Space.UI;
using JetBrains.Annotations;

namespace GanShin.Content.Creature
{
    public class RikoController : PlayerController, IAttackAnimation
    {
        private RikoStatTable _statTable;
        
        private bool _isOnUltimate;
        
        protected override void Awake()
        {
            base.Awake();
            
            _statTable = Stat as RikoStatTable;
            if (_statTable == null)
            {
                GanDebugger.LogError("Stat asset is not RikoStatTable");
                return;
            }
        }

        public override PlayerAvatarContext GetPlayerContext =>
            Player.GetAvatarContext(Define.ePlayerAvatar.RIKO);

#region Attack
        protected override void Attack()
        {
            bool  isTryAttack  = false;
            float attackDelay  = 1f;
            bool  isLastAttack = false;
            
            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                    if (!_isOnUltimate)
                    {
                        PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTACK1;
                        ObjAnimator.SetInteger(AnimPramHashAttackState, 1);
                        attackDelay = _statTable.attack1Delay;
                    }
                    else
                    {
                        PlayerAttack = ePlayerAttack.RIKO_ULTIMATE_ATTACK1;
                        ObjAnimator.SetInteger(AnimPramHashAttackState, 5);
                        attackDelay = _statTable.attack1Delay;
                    }

                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTACK1:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTACK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 2);
                    attackDelay = _statTable.attack2Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTACK2:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTACK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 3);
                    attackDelay = _statTable.attack3Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTACK3:
                    PlayerAttack = ePlayerAttack.RIKO_BASIC_ATTACK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 4);
                    attackDelay  = _statTable.attack4Delay;
                    isTryAttack  = true;
                    isLastAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK1:
                    PlayerAttack = ePlayerAttack.RIKO_ULTIMATE_ATTACK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 6);
                    attackDelay = _statTable.attack2Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK2:
                    PlayerAttack = ePlayerAttack.RIKO_ULTIMATE_ATTACK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 7);
                    attackDelay = _statTable.attack3Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK3:
                    PlayerAttack = ePlayerAttack.RIKO_ULTIMATE_ATTACK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 8);
                    attackDelay  = _statTable.attack4Delay;
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
            ReturnToIdle(_statTable.skillDelay).Forget();
        }

        protected override void UltimateSkill()
        {
            _isOnUltimate = true;
            UltimateTimer().Forget();
            Weapon.OnUltimate();
            
            PlayerAttack = ePlayerAttack.NONE;
            CharacterCutScene.OnCharacterCutScene(Define.ePlayerAvatar.RIKO);
        }

        private async UniTask UltimateTimer()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.ultimateDuration));
            _isOnUltimate = false;
            Weapon.OnUltimateEnd();
            
            PlayerAttack = ePlayerAttack.NONE;
        }

        protected override void SpecialAction()
        {
            //TODO: dash
        }
#endregion Attack

#region ActionEvent
        protected override void OnAttack(bool value)
        {
            if (!value) return;
            base.OnAttack(true);
        }
        
        protected override void OnBaseSkill(bool value)
        {
            if (!value) return;
            base.OnBaseSkill(true);
        }
        
        protected override void OnUltimateSkill(bool value)
        {
            if (!value) return;
            base.OnUltimateSkill(true);
        }
#endregion ActionEvent

#region AnimEvents
        [UsedImplicitly]
        public void OnAnimAttack()
        {
            Weapon.OnAttack();
        }

        [UsedImplicitly]
        public void OnAnimSkill()
        {
            Weapon.OnSkill();   
        }
#endregion AnimEvents
    }
}
