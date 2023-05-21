using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Data
{
    public class CharacterStatTable : ScriptableObject
    {
        [Header("Common")] 
        public float hp = 100f;
        public float baseSkillCoolTime           = 10f;
        public float moveSpeed                   = 4f;
        public float dashSpeed                   = 6f;
        public float attackCoolTime              = 0.8f;
        public float previousAttackStateHoldTime = 4f;
        
        [Header("Ultimate Skill")]
        public float ultimateSkillAvailabilityGauge  = 30f;
        public float ultimateSkillChargeOnBaseAttack = 2f;
        public float ultimateSkillChargeOnDamaged    = 3f;
        public float ultimateSkillChargeOnKill       = 5f;
        public float ultimateSkillChargeOnSkill      = 5f;
    }
}