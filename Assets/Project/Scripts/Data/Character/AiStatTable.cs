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
        public float aiProjectileSpeed = 10f;
        public float aiProjectileDuration = 7f;
        public float aiProjectileDamage = 15f;
        public float aiProjectileRadius = 0.5f;
        public float aiProjectileDetectRadius = 3f;
        public float aiProjectileMonsterHeightRatio = 0.7f;
        
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
        public float   ultimateCameraDelay    = 3f;
        public float   ultimateDuration       = 10f;
        public Vector3 ultimatePositionOffset = new(0, 1.3f, -2f);

        public float ultimateHitDamage = 40;
        public float ultimateHitRadius = 5f;
    }
}