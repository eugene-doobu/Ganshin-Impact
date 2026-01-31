#nullable enable

using GanShin.Data;
using GanShin.Effect;
using UnityEngine;

namespace GanShin.Content.Weapon
{
    public class AiStaff : PlayerWeaponBase
    {
        [SerializeField] private Transform spawnSocket = null!;
        
        public override void OnAttack()
        {
            var stat = Owner.Stat as AiStatTable;
            if (stat == null)
            {
                GanDebugger.LogError("Stat asset is not AiStatTable");
                return;
            }
            
            MakeProjectile();
        }
        
        private void MakeProjectile()
        {
            var effectManager = ProjectManager.Instance.GetManager<EffectManager>();
            if (effectManager == null)
            {
                GanDebugger.ActorLogError("EffectManager is null");
                return;
            }

            effectManager.PlayEffect(eEffectType.AI_PROJECTILE, Owner.transform.position);      
        }
    }
}
