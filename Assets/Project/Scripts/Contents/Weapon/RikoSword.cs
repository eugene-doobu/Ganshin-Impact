using UnityEngine;
using GanShin.Content.Creature.Monster;
using GanShin.Data;

namespace GanShin.Content.Weapon
{
    public class RikoSword : PlayerWeaponBase
    {
        private readonly Collider[] _monsterCollider = new Collider[20];

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
            var len = Physics.OverlapSphereNonAlloc(attackPosition, stat.rikoAttackRadius, _monsterCollider, Define.GetLayerMask(Define.eLayer.MONSTER));

            for (var i = 0; i < len; ++i)
            {
                var monster = _monsterCollider[i].GetComponent<MonsterController>();
                if (ReferenceEquals(monster, null)) continue;

                switch (AttackType)
                {
                    case ePlayerAttack.RIKO_BASIC_ATTAK1:
                    case ePlayerAttack.RIKO_BASIC_ATTAK2:
                    case ePlayerAttack.RIKO_BASIC_ATTAK3:
                    case ePlayerAttack.RIKO_BASIC_ATTAK4:
                        Owner.CurrentUltimateGauge += Owner.Stat.ultimateSkillChargeOnBaseAttack;
                        break;
                }
                
                switch (AttackType)
                {
                    case ePlayerAttack.RIKO_BASIC_ATTAK1:
                        monster.OnDamaged(stat.attack1Damage);
                        break;
                    case ePlayerAttack.RIKO_BASIC_ATTAK2:
                        monster.OnDamaged(stat.attack2Damage);
                        break;
                    case ePlayerAttack.RIKO_BASIC_ATTAK3:
                        monster.OnDamaged(stat.attack3Damage);
                        break;
                    case ePlayerAttack.RIKO_BASIC_ATTAK4:
                        monster.OnDamaged(stat.attack4Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTI_ATTAK1:
                        monster.OnDamaged(stat.ultimate1Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTI_ATTAK2:
                        monster.OnDamaged(stat.ultimate2Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTI_ATTAK3:
                        monster.OnDamaged(stat.ultimate3Damage);
                        break;
                    case ePlayerAttack.RIKO_ULTI_ATTAK4:
                        monster.OnDamaged(stat.ultimate4Damage);
                        break;
                }
            }
        }
    }
}