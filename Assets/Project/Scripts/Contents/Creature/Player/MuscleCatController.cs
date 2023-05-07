using System.Collections;
using System.Collections.Generic;
using GanShin.Data;
using UnityEngine;

namespace GanShin.Content.Creature
{
    public class MuscleCatController : PlayerController
    {
        private MuscleCatStatTable _statTable;
        
#region Attack
        protected override void Awake()
        {
            base.Awake();
            
            _statTable = Stat as MuscleCatStatTable;
            if (_statTable == null)
            {
                GanDebugger.LogError("Stat asset is not MuscleCatStatTable");
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
#endregion Attack

#region ActionEvent
        protected override void OnAttack(bool value)
        {
        }

        protected override void OnBaseSkill(bool value)
        {
            
        }

        protected override void OnUltimateSkill(bool value)
        {
            
        }
#endregion ActionEvent
    }
}
