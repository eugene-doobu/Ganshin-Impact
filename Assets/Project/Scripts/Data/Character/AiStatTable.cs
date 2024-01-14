using UnityEngine;

namespace GanShin.Data
{
    [CreateAssetMenu(menuName = "DataTable/AiStat")]
    public class AiStatTable : CharacterStatTable
    {
        [Header("Attack")] 
        public float aiAttackCooldown = 0.4f;
        public float attack1Delay = 1f;
        public float attack2Delay = 1.2f;
        
        [Header("Skill2")]
        public float skill2Duration = 5f;
        public float skill2Radius = 5f;
        public float skill2HealAmount = 5f;
        public float skill2HealDelay = 0.1f;
        public float skill2YPositionOffset = 0.1f;
    }
}