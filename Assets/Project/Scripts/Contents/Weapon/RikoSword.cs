using UnityEngine;
using GanShin.Content.Creature.Monster;
using GanShin.Data;

namespace GanShin.Content.Weapon
{
    public class RikoSword : PlayerWeaponBase
    {
        private readonly Collider[] _monsterCollider = new Collider[20];

        [SerializeField] private  MeshRenderer meshRenderer;
        
        [SerializeField] private ParticleSystem skillEffect;
        [SerializeField] private ParticleSystem ultimateEffect;
        [SerializeField] private ParticleSystem ultimateEndEffect;
        
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material ultimateMaterial;

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
                    case ePlayerAttack.RIKO_BASIC_ATTACK2:
                    case ePlayerAttack.RIKO_BASIC_ATTACK3:
                    case ePlayerAttack.RIKO_BASIC_ATTACK4:
                        Owner.CurrentUltimateGauge += Owner.Stat.ultimateSkillChargeOnBaseAttack;
                        break;
                }
                
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

            var ownerTr = Owner.transform;
            var len = Physics.OverlapSphereNonAlloc(ownerTr.position, stat.rikoSkillAttackRadius, _monsterCollider, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; ++i)
            {
                var monster = _monsterCollider[i].GetComponent<MonsterController>();
                if (ReferenceEquals(monster, null)) continue;
                
                skillEffect.Play();
                
                monster.OnDamaged(stat.skillDamage);
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