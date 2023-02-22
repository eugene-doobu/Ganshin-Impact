using System;
using System.Collections.Generic;
using UnityEngine;
using GanShin.Content.Creature.Monster;
using GanShin.Data;

namespace GanShin.Content.Weapon
{
    public class RikoSword : PlayerWeaponBase
    {
        private List<MonsterController> _monsters = new List<MonsterController>();

        protected void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Define.Tag.Monster)) return;
            if (!other.gameObject.TryGetComponent<MonsterController>(out var monster))
                return;
            _monsters.Add(monster);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Define.Tag.Monster)) return;
            if (!other.gameObject.TryGetComponent<MonsterController>(out var monster))
                return;
            _monsters.Remove(monster);
        }

        public override void OnAttack()
        {
            var stat = Owner.Stat as RikoStatTable;
            if (stat == null)
            {
                GanDebugger.LogError("Stat asset is not RikoStatTable");
                return;
            }

            foreach (var monster in _monsters)
            {
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
