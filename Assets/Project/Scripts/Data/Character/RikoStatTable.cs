using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Data
{
    [CreateAssetMenu(menuName = "DataTable/RikoStat")]
    public class RikoStatTable : CharacterStatTable
    {
        [Header("Attack")] 
        public float rikoAttackForwardOffset  = 0.5f;
        public float rikoAttackRadius            = 1.5f;
        public float rikoUltimateAttackRadius    = 2.5f;
        
        [Header("Damages")] 
        public float attack1Damage = 10;
        public float attack2Damage = 20;
        public float attack3Damage = 30;
        public float attack4Damage = 40;

        public float ultimate1Damage = 30;
        public float ultimate2Damage = 40;
        public float ultimate3Damage = 50;
        public float ultimate4Damage = 60;
        
        [Header("Attack Delay")]
        public float attack1Delay = 1f;
        public float attack2Delay = 1.2f;
        public float attack3Delay = 1.4f;
        public float attack4Delay = 1.6f;

        public float skillDelay = 1.5f;
    }
}