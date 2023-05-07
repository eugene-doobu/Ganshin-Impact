using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using GanShin.Effect;
using UnityEngine;
using Zenject;

namespace GanShin.Content.Creature
{
    public class MuscleCatController : PlayerController
    {        
        [Inject] private EffectManager _effect;

        private readonly Collider[] _monsterCollider = new Collider[10];
        
        [SerializeField] private CinemachineImpulseSource impulseSource;

        private MuscleCatStatTable _statTable;
        
        protected override void Awake()
        {
            base.Awake();
            
            _statTable = Stat as MuscleCatStatTable;
            if (_statTable == null)
            {
                GanDebugger.LogError("Stat asset is not MuscleCatStatTable");
                return;
            }
        }
        
#region Attack
        protected override void Attack()
        {
            bool  isTryAttack  = false;
            float attackDelay  = 1f;
            bool  isLastAttack = false;
            
            switch (PlayerAttack)
            {
                case ePlayerAttack.NONE:
                    PlayerAttack = ePlayerAttack.MUSCLE_CAT_ATTACK1;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 1);
                    attackDelay = _statTable.attack1Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.MUSCLE_CAT_ATTACK1:
                    PlayerAttack = ePlayerAttack.MUSCLE_CAT_ATTACK2;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 2);
                    attackDelay = _statTable.attack2Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.MUSCLE_CAT_ATTACK2:
                    PlayerAttack = ePlayerAttack.MUSCLE_CAT_ATTACK3;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 3);
                    attackDelay = _statTable.attack3Delay;
                    isTryAttack = true;
                    break;
                case ePlayerAttack.MUSCLE_CAT_ATTACK3:
                    PlayerAttack = ePlayerAttack.MUSCLE_CAT_ATTACK4;
                    ObjAnimator.SetInteger(AnimPramHashAttackState, 4);
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
                OnAttack();
                impulseSource.GenerateImpulseWithForce(_statTable.baseAttackShakeForce);
            }
        }

        private void OnAttack()
        {
            var tr             = transform;
            var damage         = GetAttackDamage();
            var attackPosition = tr.position + tr.forward * _statTable.attackForwardOffset;
            var attackRadius   = _statTable.attackRadius;
            var len = Physics.OverlapSphereNonAlloc(attackPosition, attackRadius, _monsterCollider, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; ++i)
            {
                var monster = _monsterCollider[i].GetComponent<MonsterController>();
                if (ReferenceEquals(monster, null)) continue;
                
                monster.OnDamaged(damage);
                
                var closetPoint = _monsterCollider[i].ClosestPoint(tr.position + tr.up * _statTable.attackEffectYupPosition);
                _effect.PlayEffect(eEffectType.MUSCLE_CAT_HIT, closetPoint);
            }
        }

        protected override void Skill()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);
            SkillAsync().Forget();
        }
        
        private async UniTask SkillAsync()
        {
            var len = Physics.OverlapSphereNonAlloc(transform.position, _statTable.skillRadius, _monsterCollider, Define.GetLayerMask(Define.eLayer.MONSTER));
            var monsters = new MonsterController[len];
            
            for (var i = 0; i < len; ++i)
                monsters[i] = _monsterCollider[i].GetComponent<MonsterController>();

            foreach (var monster in monsters)
            {
                var monsterTr               = monster.transform;
                var playerPositionOfMonster = transform.InverseTransformPoint(monsterTr.position);
                var knockBackPower          = Mathf.Max(-playerPositionOfMonster.z + _statTable.skillKnockBackDistance, 0f);
                monster.SetCaught();
                monsterTr.DOMove(monsterTr.position + transform.forward * knockBackPower, _statTable.skillKnockBackDuration)
                    .SetEase(_statTable.skillEaseType);
            }

            await UniTask.Delay(TimeSpan.FromMilliseconds(_statTable.skillDuration));

            foreach (var monster in monsters)
                monster.OnDamaged(_statTable.skillDamage);
        }

        protected override void UltimateSkill()
        {
            // TODO
        }
#endregion Attack

#region Attack Util
        private float GetAttackDamage()
        {
            float damage = 0;
            switch (PlayerAttack)
            {
                case ePlayerAttack.MUSCLE_CAT_ATTACK1:
                    damage = _statTable.attack1Damage;
                    break;
                case ePlayerAttack.MUSCLE_CAT_ATTACK2:
                    damage = _statTable.attack2Damage;
                    break;
                case ePlayerAttack.MUSCLE_CAT_ATTACK3:
                    damage = _statTable.attack3Damage;
                    break;
                case ePlayerAttack.MUSCLE_CAT_ATTACK4:
                    damage = _statTable.attack4Damage;
                    break;
            }

            return damage;
        }
#endregion Attack Util

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
            // TODO
        }
#endregion ActionEvent
    }
}
