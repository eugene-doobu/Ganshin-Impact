using System;
using System.Collections;
using System.Collections.Generic;
using GanShin.Content.Creature;
using UnityEngine;

namespace GanShin.Content.Weapon
{
    public abstract class PlayerWeaponBase : MonoBehaviour
    {
        public PlayerController Owner      { get; set; }
        public ePlayerAttack    AttackType { get; set; }

        public abstract void OnAttack();

        public abstract void OnSkill();
        
        public abstract void OnUltimate();

        public virtual void OnSkillEnd()
        {
        }

        public virtual void OnUltimateEnd()
        {
            
        }
    }
}