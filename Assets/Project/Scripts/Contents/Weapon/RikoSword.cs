using System;
using Cinemachine;
using UnityEngine;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using GanShin.Effect;
using Zenject;

namespace GanShin.Content.Weapon
{
    public class RikoSword : PlayerWeaponBase
    {
        [Inject] private EffectManager _effect;
        
        private readonly Collider[] _monsterCollider = new Collider[20];

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
            var len = Physics.OverlapSphereNonAlloc(attackPosition, attackRadius, _monsterCollider, Define.GetLayerMask(Define.eLayer.MONSTER));

            for (var i = 0; i < len; ++i)
            {
                var monster = _monsterCollider[i].GetComponent<MonsterController>();

                if (ReferenceEquals(monster, null)) continue;
                
                switch (AttackType)
                {
                    case ePlayerAttack.RIKO_BASIC_ATTACK1:
                        monster.OnDamaged(stat.attack1Damage);
                        break;
                    case ePlayerAttack.RIKO_BASIC_ATTACK2:
                        monster.OnDamaged(stat.attack2Damage);
                        break;
                    case ePlayerAttack.RIKO_BASIC_ATTACK3:
                        monster.OnDamaged(stat.attack3Damage);
                        break;
                    case ePlayerAttack.RIKO_BASIC_ATTACK4:
                        monster.OnDamaged(stat.attack4Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTIMATE_ATTACK1:
                        monster.OnDamaged(stat.ultimate1Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTIMATE_ATTACK2:
                        monster.OnDamaged(stat.ultimate2Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTIMATE_ATTACK3:
                        monster.OnDamaged(stat.ultimate3Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTIMATE_ATTACK4:
                        monster.OnDamaged(stat.ultimate4Damage);
                        break;
                }

                var closetPoint = _monsterCollider[i].ClosestPoint(transform.position);
                if (_isOnUltimate)
                {
                    _effect.PlayEffect(eEffectType.RIKO_SWORD_ULTIMATE_HIT, closetPoint);
                    impulseSource.GenerateImpulseWithForce(stat.rikoUltimateAttackShakeForce);
                }
                else
                {
                    _effect.PlayEffect(eEffectType.RIKO_SWORD_HIT, closetPoint);
                    Owner.CurrentUltimateGauge += Owner.Stat.ultimateSkillChargeOnBaseAttack;
                    impulseSource.GenerateImpulseWithForce(stat.rikoBaseAttackShakeForce);
                }
            }
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

            var ownerTr = Owner.transform;
            var len = Physics.OverlapSphereNonAlloc(ownerTr.position, stat.rikoSkillAttackRadius, _monsterCollider, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; ++i)
            {
                var monster = _monsterCollider[i].GetComponent<MonsterController>();
                if (ReferenceEquals(monster, null)) continue;
                
                monster.OnDamaged(stat.skillDamage);
                
                var closetPoint = _monsterCollider[i].ClosestPoint(transform.position);
                _effect.PlayEffect(eEffectType.RIKO_SWORD_HIT, closetPoint);
            }
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