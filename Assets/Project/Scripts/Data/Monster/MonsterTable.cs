using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Data
{
    public abstract class MonsterTable : ScriptableObject
    {
        [Header("Common")] 
        public float hp    = 100f;
        public float sight = 8f;
    }
}
