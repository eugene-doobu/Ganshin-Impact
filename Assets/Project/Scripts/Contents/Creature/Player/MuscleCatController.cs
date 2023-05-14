using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GanShin.CameraSystem;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using GanShin.Effect;
using UnityEngine;
using Zenject;

namespace GanShin.Content.Creature
{
    public class MuscleCatController : PlayerController
    {   
        private static readonly int AnimPramHashIsOnGuard = Animator.StringToHash("IsOnGuard");
        private static readonly int AnimPramHashSetPunch  = Animator.StringToHash("SetPunch");
        
        [Inject] private EffectManager _effect;
        [Inject] private CameraManager _camera;

        private readonly Collider[] _monsterColliders = new Collider[30];
        
        [SerializeField] private CinemachineImpulseSource baseAttackImpulseSource;
        [SerializeField] private CinemachineImpulseSource ultimateImpulseSource;

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
            }
        }

        private void OnAttack()
        {
            var tr             = transform;
            var damage         = GetAttackDamage();
            var attackPosition = tr.position + tr.forward * _statTable.attackForwardOffset;
            var attackRadius   = _statTable.attackRadius;
            var rst = ApplyAttackDamage(attackPosition, attackRadius, damage, _monsterColliders, OnAttackEffect);
            if (!rst) return;
            
            CurrentUltimateGauge += _statTable.ultimateSkillChargeOnBaseAttack;
            baseAttackImpulseSource.GenerateImpulseWithForce(_statTable.baseAttackShakeForce);
        }

        private void OnAttackEffect(Collider monsterCollider)
        {
            var tr          = transform;
            var closetPoint = monsterCollider.ClosestPoint(tr.position + tr.up * _statTable.attackEffectYupPosition);
            _effect.PlayEffect(eEffectType.MUSCLE_CAT_HIT, closetPoint);
        }

        protected override void Skill()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);
            SkillAsync().Forget();
        }

        public override void OnDamaged(float damage)
        {
            if (IsOnSpecialAction)
            {
                var tr = transform;
                _effect.PlayEffect(eEffectType.MUSCLE_CAT_HIT, tr.position + tr.up * _statTable.attackEffectYupPosition);
                base.OnDamaged(0);
                return;
            }
            base.OnDamaged(damage);
        }

        private async UniTask SkillAsync()
        {
            var len = Physics.OverlapSphereNonAlloc(transform.position, _statTable.skillRadius, _monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
            var monsters = new MonsterController[len];
            
            for (var i = 0; i < len; ++i)
                monsters[i] = _monsterColliders[i].GetComponent<MonsterController>();

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

            CurrentUltimateGauge += _statTable.ultimateSkillChargeOnBaseAttack;
        }

        protected override void UltimateSkill()
        {
            UltimateSkillAsync().Forget();
        }

        private async UniTask UltimateSkillAsync()
        {
            CharacterCutScene.OnCharacterCutScene(Define.ePlayerAvatar.MUSCLE_CAT);

            _camera.ChangeState(eCameraState.CHARACTER_ULTIMATE_CAMERA);
            ObjAnimator.SetTrigger(AnimPramHashOnUltimate);

            var rightHandTr = ObjAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            _effect.PlayEffect(eEffectType.MUSCLE_CAT_ULTIMATE, rightHandTr.position);

            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.ultimateChargeDelay));
            ObjAnimator.SetTrigger(AnimPramHashSetPunch);
            _camera.ChangeState(eCameraState.CHARACTER_CAMERA);

            ultimateImpulseSource.GenerateImpulseWithForce(_statTable.ultimateShakeForce);
            
            var tr             = transform;
            var attackPosition = tr.position + tr.forward * _statTable.ultimateForwardOffset;
            ApplyAttackDamage(attackPosition, _statTable.ultimateRadius, _statTable.ultimateDamage, _monsterColliders, OnAttackEffect);
            
            PlayerAttack = ePlayerAttack.NONE;
        }

        protected override void SpecialAction()
        {
            ObjAnimator.SetBool(AnimPramHashIsOnGuard, IsOnSpecialAction);
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
            if (!value) return;
            base.OnUltimateSkill(true);
        }
#endregion ActionEvent
    }
}
