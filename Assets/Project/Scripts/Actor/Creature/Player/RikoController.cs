using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Data;
using GanShin.Effect;
using GanShin.Space.UI;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class RikoController : PlayerController, IAttackAnimation
    {
        private bool          _isOnUltimate;
        private RikoStatTable _statTable;

        private float _rikoAttackCooldown;

        protected override void Awake()
        {
            PlayerType = Define.ePlayerAvatar.RIKO;
            
            base.Awake();

            _statTable = Stat as RikoStatTable;
            if (_statTable == null) GanDebugger.LogError("Stat asset is not RikoStatTable");
        }

        public override void Tick()
        {
            base.Tick();
            _rikoAttackCooldown = Mathf.Clamp(_rikoAttackCooldown - Time.deltaTime, 0, _statTable.rikoAttackCooldown);
        }

#region Attack
        protected override void Attack()
        {
            if (_rikoAttackCooldown > 0) return;
            _rikoAttackCooldown += _statTable.rikoAttackCooldown;
            
            var isTryAttack  = false;
            var attackDelay  = 1f;
            var isLastAttack = false;

            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                    PlayerAttack = !_isOnUltimate ? ePlayerAttack.RIKO_BASIC_ATTACK1 : ePlayerAttack.RIKO_ULTIMATE_ATTACK1;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, (int)PlayerAttack);
                    attackDelay = _statTable.attack1Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTACK1:
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK1:
                    PlayerAttack += 1;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, (int)PlayerAttack);
                    attackDelay = _statTable.attack2Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTACK2:
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK2:
                    PlayerAttack += 1;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, (int)PlayerAttack);
                    attackDelay = _statTable.attack3Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.RIKO_BASIC_ATTACK3:
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK3:
                    PlayerAttack += 1;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, (int)PlayerAttack);
                    attackDelay  = _statTable.attack4Delay;
                    isTryAttack  = true;
                    isLastAttack = true;
                    break;
            }

            if (!isTryAttack) return;
            
            CanMove = false;
            if (AttackCancellationTokenSource != null)
                DisposeAttackCancellationTokenSource();
            AttackCancellationTokenSource = new CancellationTokenSource();
            ReturnToIdle(attackDelay, isLastAttack).Forget();
        }

        protected override void Skill()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);

            CanMove = false;
            if (AttackCancellationTokenSource != null)
                DisposeAttackCancellationTokenSource();
            AttackCancellationTokenSource = new CancellationTokenSource();
            ReturnToIdle(_statTable.skillDelay).Forget();
        }

        protected override void Skill2()
        {
            var particle = ProjectManager.Instance.GetManager<EffectManager>()?.PlayEffect(eEffectType.RIKO_SKILL2, transform.position);
            if (particle == null)
            {
                GanDebugger.ActorLogError("Failed to get particle");
                return;
            }
            
            var skillTr = particle.transform;
            var rikoTr  = transform;
            
            skillTr.SetParent(rikoTr);
            skillTr.localPosition = _statTable.skill2Offset;
            skillTr.localRotation = Quaternion.identity;
            skillTr.SetParent(null);
        }

        protected override void UltimateSkill()
        {
            _isOnUltimate = true;
            UltimateTimer().Forget();
            Weapon.OnUltimate();

            PlayerAttack = ePlayerAttack.NONE;

            ShowCutScene();
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