using DG.Tweening;
using UnityEngine;

namespace GanShin.Data
{
    [CreateAssetMenu(menuName = "DataTable/MuscleCatStat")]
    public class MuscleCatStatTable : CharacterStatTable
    {
        [Header("Attack")] 
        public float attackForwardOffset = 0.3f;
        public float attackRadius            = 1.0f;
        public float attackEffectYupPosition = 1.2f;
        public float effectNoiseMaxValue     = 0.2f;

        [Header("Camera Shake")] 
        public float baseAttackShakeForce = 0.03f;
        public float ultimateShakeForce = 3f;
        
        [Header("Attack Damages")] 
        public float attack1Damage = 10;
        public float attack2Damage = 15;
        public float attack3Damage = 20;
        public float attack4Damage = 25;

        [Header("Attack Delay")]
        public float attack1Delay = 0.15f;
        public float attack2Delay = 0.15f;
        public float attack3Delay = 0.15f;
        public float attack4Delay = 0.24f;

        [Header("Skill")] 
        public float skillRadius   = 3.0f;
        public float skillDamage   = 40;
        public float skillDuration = 0.3f;
        
        [Header("Skill2")]
        public float skill2Radius = 2.0f;
        public float skill2AttackDelay = 0.15f;
        public float skill2Damage   = 8f;
        public float skill2Duration = 2f;
        
        [Header("Ultimate")]
        public float ultimateForwardOffset = 1.5f;
        public float ultimateRadius      = 7f;
        public float ultimateDamage      = 100f;
        public float ultimateChargeDelay = 1.5f;
    }
}