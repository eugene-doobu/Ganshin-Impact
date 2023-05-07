using Cinemachine;
using UnityEngine;
using GanShin.Data;
using GanShin.Effect;
using GanShin.Sound;
using Zenject;
using Random = UnityEngine.Random;

namespace GanShin.Content.Weapon
{
    public class RikoSword : PlayerWeaponBase
    {
        [Inject] private EffectManager _effect;
        [Inject] private SoundManager  _sound;
        
        private readonly Collider[] _monsterColliders = new Collider[20];

        [SerializeField] private  MeshRenderer meshRenderer;
        
        [SerializeField] private ParticleSystem skillEffect;
        [SerializeField] private ParticleSystem ultimateEffect;
        [SerializeField] private ParticleSystem ultimateEndEffect;
        
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material ultimateMaterial;
        
        [SerializeField] private CinemachineImpulseSource impulseSource;
        
        private bool _isOnUltimate;

        public override void OnAttack()
        {
            var stat = Owner.Stat as RikoStatTable;
            if (stat == null)
            {
                GanDebugger.LogError("Stat asset is not RikoStatTable");
                return;
            }

            var ownerTr = Owner.transform;
            var attackPosition = ownerTr.position + ownerTr.forward * stat.rikoAttackForwardOffset;
            var attackRadius = _isOnUltimate ? stat.rikoUltimateAttackRadius : stat.rikoAttackRadius;

            _sound.Play(_isOnUltimate
                ? $"Sword/Staff/Staff {Random.Range(1, 11)}"
                : $"Sword/Club/Club {Random.Range(1, 11)}");
            
            var rst = Owner.ApplyAttackDamage(attackPosition, attackRadius, GetBaseAttackDamage(stat), _monsterColliders, OnBaseAttackEffect);
            if (!rst) return;

            if (_isOnUltimate)
            {
                impulseSource.GenerateImpulseWithForce(stat.rikoUltimateAttackShakeForce);
            }
            else
            {
                impulseSource.GenerateImpulseWithForce(stat.rikoBaseAttackShakeForce);
                Owner.CurrentUltimateGauge += Owner.Stat.ultimateSkillChargeOnBaseAttack;
            }
        }
        
        private void OnBaseAttackEffect(Collider monsterCollider)
        {
            var closetPoint = monsterCollider.ClosestPoint(transform.position);
            _effect.PlayEffect(_isOnUltimate ? eEffectType.RIKO_SWORD_ULTIMATE_HIT : eEffectType.RIKO_SWORD_HIT,
                closetPoint);
        }

        private float GetBaseAttackDamage(RikoStatTable stat)
        {
            switch (AttackType)
            {
                case ePlayerAttack.RIKO_BASIC_ATTACK1:
                    return stat.attack1Damage;
                case ePlayerAttack.RIKO_BASIC_ATTACK2:
                    return stat.attack2Damage;
                case ePlayerAttack.RIKO_BASIC_ATTACK3:
                    return stat.attack3Damage;
                case ePlayerAttack.RIKO_BASIC_ATTACK4:
                    return stat.attack4Damage;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK1:
                    return stat.ultimate1Damage;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK2:
                    return stat.ultimate2Damage;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK3:
                    return stat.ultimate3Damage;
                case ePlayerAttack.RIKO_ULTIMATE_ATTACK4:
                    return stat.ultimate4Damage;
            }
            return 0;
        }

        public override void OnSkill()
        {
            var stat = Owner.Stat as RikoStatTable;
            if (stat == null)
            {
                GanDebugger.LogError("Stat asset is not RikoStatTable");
                return;
            }
            
            skillEffect.Play();
            impulseSource.GenerateImpulseWithForce(stat.rikoSkillShakeForce);
            _sound.Play("Riko/OnSkill");

            Owner.ApplyAttackDamage(Owner.transform.position, stat.rikoSkillAttackRadius, stat.skillDamage, _monsterColliders,
                OnBaseSkillEffect);
        }

        private void OnBaseSkillEffect(Collider monsterCollider)
        {
            var closetPoint = monsterCollider.ClosestPoint(transform.position);
            _effect.PlayEffect(eEffectType.RIKO_SWORD_HIT, closetPoint);
        }

        public override void OnUltimate()
        {
            var stat = Owner.Stat as RikoStatTable;
            if (stat == null)
            {
                GanDebugger.LogError("Stat asset is not RikoStatTable");
                return;
            }
            
            ultimateEndEffect.Play();
            meshRenderer.material = ultimateMaterial;
            transform.localScale = stat.ultimateSwordScale;
            ultimateEffect.Play();
            _sound.Play("Riko/OnUltimate");

            _isOnUltimate = true;
            
            impulseSource.GenerateImpulseWithForce(stat.rikoSkillShakeForce);
        }

        public override void OnUltimateEnd()
        {
            meshRenderer.material = defaultMaterial;
            transform.localScale  = Vector3.one;
            ultimateEffect.Stop();
            
            ultimateEndEffect.Play();
            
            _isOnUltimate = false;
        }
    }
}