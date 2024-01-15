#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.Content.Creature;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using UnityEngine;

namespace GanShin.GanObject
{
    public class AISkill1Controller : SkillObject
    {
        private readonly Collider[] _monsterColliders = new Collider[20];
        
        private AiStatTable? _stat;
        
        private float _damageCooldown;

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

        public override void Tick()
        {
            base.Tick();
            
            if (_stat == null) return;
            
            _damageCooldown = Mathf.Clamp(_damageCooldown - Time.deltaTime, 0, _stat.skill1DamageDelay);
            
            if (_damageCooldown > 0) 
                return;
            
            _damageCooldown += _stat.skill1DamageDelay;
            var len = Physics.OverlapSphereNonAlloc(transform.position, _stat.skill1Radius, _monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; i++)
            {
                var monster = _monsterColliders[i].GetComponent<MonsterController>();
                if (monster == null)
                    continue;
                
                monster.OnDamaged(_stat.skill1Damage);
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
            
            var tr        = transform;
            var playerTr  = player.transform;
            var playerPos = playerTr.position;
            var pos       = playerPos + playerTr.right * _stat.skill1PositionOffset.x;
            pos    += playerTr.up * _stat.skill1PositionOffset.y;
            pos    += playerTr.forward * _stat.skill1PositionOffset.z;
            tr.position = pos;
            tr.rotation = playerTr.rotation;

            DestroySelf().Forget();
        }

        private async UniTask DestroySelf()
        {
            if (_stat == null)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_stat.skill1Duration));
            
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stat == null)
                return;
            
            Gizmos.DrawWireSphere(transform.position, _stat.skill1Radius);
        }
#endif // UNITY_EDITOR
    }
}
