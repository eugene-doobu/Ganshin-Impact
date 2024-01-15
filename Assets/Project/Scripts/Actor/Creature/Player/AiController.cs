using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using GanShin.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class AiController : PlayerController
    {
        private AiStatTable _statTable;

        private float _aiAttackCooldown;
        private bool _isOnSkill;
        
        private readonly Collider[] _monsterColliders = new Collider[20];
        
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
            Skill1Spell().Forget();
        }

        private async UniTask Skill1Spell()
        {
            var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
            if (playerManager == null) return;
            
            CantMove();
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.skill1SpellDelay));
            if (IsDead || playerManager.CurrentPlayer != this) return;
            ReturnToIdleFromSkill();
            
            var effect = ProjectManager.Instance.GetManager<EffectManager>();
            effect?.PlayEffect(eEffectType.AI_SKILL1, transform.position);
        }

        protected override void Skill2()
        {
            Skill2Spell().Forget();
        }

        private async UniTask Skill2Spell()
        {
            var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
            if (playerManager == null) return;
            
            CantMove();
            ObjAnimator.SetTrigger(AnimPramHashOnSkill2);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.skill2SpellDelay));
            if (IsDead || playerManager.CurrentPlayer != this) return;
            ReturnToIdleFromSkill();
            
            var effect = ProjectManager.Instance.GetManager<EffectManager>();
            effect?.PlayEffect(eEffectType.AI_SKILL2, transform.position);
        }

        protected override void UltimateSkill()
        {
            UltimateSpell().Forget();
        }
        
        private async UniTask UltimateSpell()
        {
            ShowCutScene();
            var playerManager = ProjectManager.Instance.GetManager<PlayerManager>();
            var cameraManager = ProjectManager.Instance.GetManager<CameraManager>();
            if (playerManager == null) return;

            CantMove();
            ObjAnimator.SetTrigger(AnimPramHashOnUltimate);
            cameraManager?.ChangeState(eCameraState.CHARACTER_ULTIMATE_CAMERA);

            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.ultimateSpellDelay));
            if (IsDead || playerManager.CurrentPlayer != this) return;
            ReturnToIdleFromSkill();
            
            var effect = ProjectManager.Instance.GetManager<EffectManager>();
            effect?.PlayEffect(eEffectType.AI_ULTIMATE, transform.position);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.ultimateCameraDelay));
            cameraManager?.ChangeState(eCameraState.CHARACTER_CAMERA);
        }
        
        private void CantMove()
        {
            CanMove               = false;
            IsCantToIdleAnimation = true;
        }
        
        private void ReturnToIdleFromSkill()
        {
            CanMove               = true;
            IsCantToIdleAnimation = false;
            ReturnToIdle(0.01f).Forget();
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
            var len = Physics.OverlapSphereNonAlloc(transform.position, _statTable.aiDetectMonsterRadius, _monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; i++)
            {
                var monster = _monsterColliders[i].GetComponent<MonsterController>();
                if (monster == null)
                    continue;
            
                var monsterController = _monsterColliders[0].GetComponent<MonsterController>();
                transform.rotation = Quaternion.LookRotation(monsterController.transform.position - transform.position);
                break;
            }
        }
#endregion ActionEvent

#region AnimEvents
        [UsedImplicitly]
        public void OnAnimAttack()
        {
            Weapon.OnAttack();
        }
#endregion AnimEvents
    }
}