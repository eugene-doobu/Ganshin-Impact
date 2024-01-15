#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.Content.Creature;
using GanShin.Data;

namespace GanShin.GanObject
{
    public class AIUltimateController : SkillObject
    {
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
            
            var tr        = transform;
            var playerTr  = player.transform;
            var playerPos = playerTr.position;
            var pos       = playerPos + playerTr.right * _stat.ultimatePositionOffset.x;
            pos         += playerTr.up * _stat.ultimatePositionOffset.y;
            pos         += playerTr.forward * _stat.ultimatePositionOffset.z;
            tr.position =  pos;
            tr.rotation =  playerTr.rotation;

            DestroySelf().Forget();
        }

        private async UniTask DestroySelf()
        {
            if (_stat == null)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_stat.ultimateDuration));
            
            Destroy(gameObject);
        }
    }
}
