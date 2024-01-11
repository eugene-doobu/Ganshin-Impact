using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using GanShin.Effect;
using GanShin.Space.UI;
using GanShin.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GanShin.Content.Creature
{
    public class MuscleCatController : PlayerController
    {
        private static readonly int AnimPramHashIsOnGuard = Animator.StringToHash("IsOnGuard");
        private static readonly int AnimPramHashSetPunch  = Animator.StringToHash("SetPunch");

        [SerializeField] private CinemachineImpulseSource baseAttackImpulseSource;
        [SerializeField] private CinemachineImpulseSource ultimateImpulseSource;

        private readonly Collider[] _monsterColliders = new Collider[30];

        private MuscleCatStatTable _statTable;

        private EffectManager Effect => ProjectManager.Instance.GetManager<EffectManager>();
        private CameraManager Camera => ProjectManager.Instance.GetManager<CameraManager>();

        public override PlayerAvatarContext GetPlayerContext =>
            Player.GetAvatarContext(Define.ePlayerAvatar.MUSCLE_CAT);

        protected override void Awake()
        {
            base.Awake();

            _statTable = Stat as MuscleCatStatTable;
            if (_statTable == null) GanDebugger.LogError("Stat asset is not MuscleCatStatTable");
        }

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

#region Attack

        protected override void Attack()
        {
            var isTryAttack  = false;
            var attackDelay  = 1f;
            var isLastAttack = false;

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
                if (AttackCancellationTokenSource != null)
                    DisposeAttackCancellationTokenSource();
                AttackCancellationTokenSource = new CancellationTokenSource();
                ReturnToIdle(attackDelay, isLastAttack).Forget();
                OnAttack();
            }
        }

        private void OnAttack()
        {
            var tr = transform;
            var damage = GetAttackDamage();
            var attackPosition = tr.position + tr.forward * _statTable.attackForwardOffset;
            var attackRadius = _statTable.attackRadius;
            var rst = ApplyAttackDamage(attackPosition, attackRadius, damage, _monsterColliders, OnAttackEffect);
            if (!rst) return;

            CurrentUltimateGauge += _statTable.ultimateSkillChargeOnBaseAttack;
            baseAttackImpulseSource.GenerateImpulseWithForce(_statTable.baseAttackShakeForce);
        }

        private void OnAttackEffect(Collider monsterCollider)
        {
            var tr             = transform;
            var basePosition   = tr.position + tr.up * _statTable.attackEffectYupPosition;
            var noiseMaxValue  = _statTable.effectNoiseMaxValue;
            var effectPosition = basePosition + new Vector3(Random.Range(-noiseMaxValue, noiseMaxValue),
                Random.Range(-noiseMaxValue, noiseMaxValue), Random.Range(-noiseMaxValue, noiseMaxValue));
            var closetPoint    = monsterCollider.ClosestPoint(effectPosition);
            Effect.PlayEffect(eEffectType.MUSCLE_CAT_HIT, closetPoint);
        }

        protected override void Skill()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill);
            SkillAsync().Forget();
        }

        protected override void Skill2()
        {
            Skill2Async().Forget();
        }

        public override void OnDamaged(float damage)
        {
            if (IsOnSpecialAction)
            {
                var tr = transform;
                Effect.PlayEffect(eEffectType.MUSCLE_CAT_HIT, tr.position + tr.up * _statTable.attackEffectYupPosition);
                base.OnDamaged(0);
                return;
            }

            base.OnDamaged(damage);
        }

        private async UniTask SkillAsync()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(_statTable.skillDuration));
            var tr            = transform;
            var attackPosition = tr.position + tr.forward * _statTable.attackForwardOffset;
            var attackRadius   = _statTable.skillRadius;
            ApplyAttackDamage(attackPosition, attackRadius, _statTable.skillDamage, _monsterColliders, OnAttackEffect);
            CurrentUltimateGauge += _statTable.ultimateSkillChargeOnBaseAttack;
        }

        private async UniTask Skill2Async()
        {
            ObjAnimator.SetTrigger(AnimPramHashOnSkill2);
            var timer = 0f;
            while (timer < _statTable.skill2Duration)
            {
                var tr             = transform;
                var attackPosition = tr.position + tr.forward * _statTable.attackForwardOffset;
                var attackRadius   = _statTable.skill2Radius;
                ApplyAttackDamage(attackPosition, attackRadius, _statTable.skill2Damage, _monsterColliders, OnAttackEffect);

                await UniTask.Delay(TimeSpan.FromSeconds(_statTable.skill2AttackDelay));
                timer += _statTable.skill2AttackDelay;
            }
            ObjAnimator.SetTrigger(AnimPramHashSetIdle);
        }

        protected override void UltimateSkill()
        {
            UltimateSkillAsync().Forget();
        }

        private async UniTask UltimateSkillAsync()
        {
            var characterCutScene =
                ProjectManager.Instance.GetManager<UIManager>()?.GetGlobalUI(EGlobalUI.CHARACTER_CUT_SCENE) as
                    UIRootCharacterCutScene;
            if (characterCutScene != null)
                characterCutScene.OnCharacterCutScene(Define.ePlayerAvatar.MUSCLE_CAT);

            Camera.ChangeState(eCameraState.CHARACTER_ULTIMATE_CAMERA);
            ObjAnimator.SetTrigger(AnimPramHashOnUltimate);

            var rightHandTr = ObjAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            Effect.PlayEffect(eEffectType.MUSCLE_CAT_ULTIMATE, rightHandTr.position);

            await UniTask.Delay(TimeSpan.FromSeconds(_statTable.ultimateChargeDelay));
            ObjAnimator.SetTrigger(AnimPramHashSetPunch);
            Camera.ChangeState(eCameraState.CHARACTER_CAMERA);

            ultimateImpulseSource.GenerateImpulseWithForce(_statTable.ultimateShakeForce);

            var tr             = transform;
            var attackPosition = tr.position + tr.forward * _statTable.ultimateForwardOffset;
            ApplyAttackDamage(attackPosition, _statTable.ultimateRadius, _statTable.ultimateDamage, _monsterColliders,
                              OnAttackEffect);

            PlayerAttack = ePlayerAttack.NONE;
        }

        protected override void SpecialAction()
        {
            ObjAnimator.SetBool(AnimPramHashIsOnGuard, IsOnSpecialAction);
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
    }
}