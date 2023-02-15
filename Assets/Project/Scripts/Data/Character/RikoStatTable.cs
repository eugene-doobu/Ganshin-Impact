using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Data
{
    [CreateAssetMenu(menuName = "DataTable/RikoStat")]
    public class RikoStatTable : CharacterStatTable
    {
        // TODO: 공격쿨타임, 애니메이션 디테일한 설정값 추가
        
        [Header("Damages")]
        public float attack1Damage = 10;
        public float attack2Damage = 20;
        public float attack3Damage = 30;
        public float attack4Damage = 40;

        public float ultimate1Damage = 30;
        public float ultimate2Damage = 40;
        public float ultimate3Damage = 50;
        public float ultimate4Damage = 60;
    }
}
