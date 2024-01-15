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
        
        [Header("Skill1")]
        public float skill1SpellDelay = 0.5f;
        public float skill1Duration = 4f;
        public float skill1Radius = 5f;
        public float skill1Damage = 0.1f;
        public float skill1DamageDelay = 0.1f;
        public Vector3 skill1PositionOffset = new Vector3(0, 0.1f, 3f);
        
        [Header("Skill2")]
        public float skill2SpellDelay = 1.0f;
        public float skill2Duration = 5f;
        public float skill2Radius = 5f;
        public float skill2HealAmount = 5f;
        public float skill2HealDelay = 0.1f;
        public float skill2YPositionOffset = 0.1f;
        
        [Header("Ultimate")]
        public float ultimateSpellDelay = 2.3f;
    }
}