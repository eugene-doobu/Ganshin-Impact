using System.Collections;
using System.Collections.Generic;
using GanShin.Data;
using GanShin.UI;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class AiController : PlayerController
    {
        private AiStatTable _statTable;

        protected override void Awake()
        {
            base.Awake();
            
            _statTable = Stat as AiStatTable;
            if (_statTable == null)
            {
                GanDebugger.LogError("Stat asset is not AiStatTable");
                return;
            }
        }

        protected override void Attack()
        {
            
        }

        protected override void Skill()
        {
            
        }

        protected override void UltimateSkill()
        {
            
        }
    }
}
