using GanShin.Data.Core;
using UnityEngine;

namespace GanShin.Data
{
    public abstract class MonsterTable : ScriptableObject, IStatTable
    {
        [Header("Common")] public float hp    = 100f;
        public                    float sight = 8f;

        public float Hp => hp;
    }
}