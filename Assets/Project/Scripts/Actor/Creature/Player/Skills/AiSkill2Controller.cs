#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.Content.Creature;
using GanShin.Data;
using UnityEngine;

namespace GanShin.GanObject
{
    public class AiSkill2Controller : SkillObject
    {
        private readonly Collider[] _playerColliders = new Collider[3];
        
        private AiStatTable? _stat;
        
        private float _healCooldown;

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
            
            _healCooldown = Mathf.Clamp(_healCooldown - Time.deltaTime, 0, _stat.skill2HealDelay);
            
            if (_healCooldown > 0) 
                return;
            
            _healCooldown += _stat.skill2HealDelay;
            var len = Physics.OverlapSphereNonAlloc(transform.position, _stat.skill2Radius, _playerColliders, Define.GetLayerMask(Define.eLayer.CHARACTER));
            for (var i = 0; i < len; i++)
            {
                var player = _playerColliders[i].GetComponent<PlayerController>();
                if (player == null)
                    continue;
                
                player.OnHealed(_stat.skill2HealAmount);
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
            
            var tr       = transform;
            var playerTr = player.transform;
            tr.position = playerTr.position + Vector3.up * _stat.skill2YPositionOffset;
            tr.rotation = playerTr.rotation;

            DestroySelf().Forget();
        }

        private async UniTask DestroySelf()
        {
            if (_stat == null)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_stat.skill2Duration));
            
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stat == null)
                return;
            
            Gizmos.DrawWireSphere(transform.position, _stat.skill2Radius);
        }
#endif // UNITY_EDITOR
    }
}
