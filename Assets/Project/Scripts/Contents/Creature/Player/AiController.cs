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

#region Attack
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
#endregion Attack

#region ActionEvent
        protected override void OnAttack(bool value)
        {
            if (IsOnSpecialAction)
            {
                if (value) return; // 조준
                base.OnAttack(false); // 발사
            }
            else
            {
                if (!value) return;
                base.OnAttack(true); // 발사
            }
        }

        protected override void OnBaseSkill(bool value)
        {
            
        }

        protected override void OnUltimateSkill(bool value)
        {
            
        }
        protected override void SpecialAction()
        {
            //TODO: 조준
        }
#endregion ActionEvent
    }
}
