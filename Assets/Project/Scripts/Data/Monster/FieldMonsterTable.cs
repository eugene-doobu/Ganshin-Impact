using DG.Tweening;
using GanShin.Content.Creature.Monster;
using UnityEngine;

namespace GanShin.Data
{
    [CreateAssetMenu(menuName = "DataTable/FieldMonster")]
    public class FieldMonsterTable : MonsterTable
    {
        [Header("FieldMonster")]
        public eFieldMonsterType monsterType = eFieldMonsterType.DEFAULT;
        
        public float destroyDelay         = 5f;
        public float rotationSmoothFactor = 8f;
        
        [Header("attack")]
        public float attackRange          = 2f;
        public float attackDelay          = 2f;
        public float traceRange           = 12f;
        public float attackDamage         = 10f;
        public float attackDuration       = 1f;
        
        [Header("damaged")]
        public float knockDuration  = 0.5f;
        public float knockBackPower = 5f;
        public Ease  knockBackEase  = Ease.InOutSine;
        
    }
}