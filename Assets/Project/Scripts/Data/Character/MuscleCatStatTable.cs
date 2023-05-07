using UnityEngine;

namespace GanShin.Data
{
    [CreateAssetMenu(menuName = "DataTable/MuscleCatStat")]
    public class MuscleCatStatTable : CharacterStatTable
    {
        [Header("Attack")] 
        public float attackForwardOffset = 0.3f;
        public float attackRadius = 1.0f;

        [Header("Camera Shake")] 
        public float baseAttackShakeForce = 0.03f;
        
        [Header("Damages")] 
        public float attack1Damage = 10;
        public float attack2Damage = 15;
        public float attack3Damage = 20;
        public float attack4Damage = 25;
        
        [Header("Attack Delay")]
        public float attack1Delay = 0.15f;
        public float attack2Delay = 0.15f;
        public float attack3Delay = 0.15f;
        public float attack4Delay = 0.24f;
    }
}