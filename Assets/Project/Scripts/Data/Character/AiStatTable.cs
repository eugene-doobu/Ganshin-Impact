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
    }
}