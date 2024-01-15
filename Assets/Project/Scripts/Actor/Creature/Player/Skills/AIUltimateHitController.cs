#nullable enable

using GanShin.Content.Creature;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using UnityEngine;

namespace GanShin.GanObject
{
    public class AIUltimateHitController : SkillObject
    {
        private readonly Collider[] _monsterColliders = new Collider[20];
        
        private AiStatTable? _stat;

        public override CreatureObject Owner
        {
            get => base.Owner;
            set
            {
                if (value is not AiController aiController)
                {
                    GanDebugger.ActorLogError("Owner is not AiController");
                    return;
                }
                _stat = aiController.Stat as AiStatTable;
                if (_stat == null)
                {
                    GanDebugger.ActorLogError("Stat asset is not AiStatTable");
                    return;
                }
                
                base.Owner = value; 
            }
        }

        protected override void Initialize()
        {
            var player = ProjectManager.Instance.GetManager<PlayerManager>()?.GetPlayer(Define.ePlayerAvatar.AI);
            if (player == null)
            {
                GanDebugger.ActorLogError("Failed to get player");
                return;
            }
            
            base.Initialize();
            Owner = player;
            
            if (_stat == null)
            {
                GanDebugger.ActorLogError("Stat asset is not AiStatTable");
                return;
            }
            
            var len = Physics.OverlapSphereNonAlloc(transform.position, _stat.ultimateHitRadius, _monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; i++)
            {
                var monster = _monsterColliders[i].GetComponent<MonsterController>();
                if (monster == null)
                    continue;
                
                monster.OnDamaged(_stat.ultimateHitDamage);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stat == null)
                return;
            
            Gizmos.DrawWireSphere(transform.position, _stat.ultimateHitRadius);
        }
#endif // UNITY_EDITOR
    }
}
