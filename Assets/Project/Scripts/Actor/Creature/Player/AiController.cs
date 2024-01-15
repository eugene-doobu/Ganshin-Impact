using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Data;
using GanShin.Effect;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class AiController : PlayerController
    {
        private AiStatTable _statTable;

        private float _aiAttackCooldown;
        private bool _isOnSkill;
        
        protected override void Awake()
        {
            PlayerType = Define.ePlayerAvatar.AI;
            
            base.Awake();

            _statTable = Stat as AiStatTable;
            if (_statTable == null) GanDebugger.LogError("Stat asset is not AiStatTable");
        }
        
        public override void Tick()
        {
            base.Tick();
            _aiAttackCooldown = Mathf.Clamp(_aiAttackCooldown - Time.deltaTime, 0, _statTable.aiAttackCooldown);
        }

#region Attack
        protected override void Attack()
        {
            if (_aiAttackCooldown > 0) return;
            _aiAttackCooldown += _statTable.aiAttackCooldown;
            
            if (_isOnSkill) 
                return;
            
            var isTryAttack  = false;
            var attackDelay  = 1f;

            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                case ePlayerAttack.AI_ATTACK2:
                    PlayerAttack = ePlayerAttack.AI_ATTACK1;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 1);
                    attackDelay = _statTable.attack1Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.AI_ATTACK1:
                    PlayerAttack = ePlayerAttack.AI_ATTACK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 2);
                    attackDelay  = _statTable.attack2Delay;
                    isTryAttack  = true;
                    break;
            }

            if (isTryAttack)
            {
                CanMove = false;
                if (AttackCancellationTokenSource != null)
                    DisposeAttackCancellationTokenSource();
                AttackCancellationTokenSource = new CancellationTokenSource();
                
                ReturnToIdle(attackDelay, false).Forget();
            }
        }

        protected override void Skill()
        {
            var effect = ProjectManager.Instance.GetManager<EffectManager>();
            effect?.PlayEffect(eEffectType.AI_SKILL1, transform.position);
        }

        protected override void Skill2()
        {
            var effect = ProjectManager.Instance.GetManager<EffectManager>();
            effect?.PlayEffect(eEffectType.AI_SKILL2, transform.position);
        }

        protected override void UltimateSkill()
        {
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

        protected override void SpecialAction()
        {
            //TODO: 조준
        }
#endregion ActionEvent
    }
}