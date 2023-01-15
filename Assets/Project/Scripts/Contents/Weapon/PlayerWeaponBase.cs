using System;
using System.Collections;
using System.Collections.Generic;
using GanShin.Creature;
using UnityEngine;

namespace GanShin.Content.Weapon
{
    public abstract class PlayerWeaponBase : MonoBehaviour
    {
        public PlayerController Owner { get; set; }
        public ePlayerAttack AttackType { get; set; }

        protected void OnTriggerEnter(Collider other)
        {
            GanDebugger.Log($"OnTriggerEnter : {other.name}");
        }

        protected void OnTriggerExit(Collider other)
        {
            GanDebugger.Log($"OnTriggerExit : {other.name}");
        }
    }
}
