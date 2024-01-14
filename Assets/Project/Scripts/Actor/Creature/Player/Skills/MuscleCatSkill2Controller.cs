#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.Content.Creature;
using GanShin.Data;
using UnityEngine;

namespace GanShin.GanObject
{
    public class MuscleCatSkill2Controller : SkillObject
    {
        private MuscleCatStatTable? _stat;

        public override CreatureObject Owner
        {
            get => base.Owner;
            set
            {
                if (value is not MuscleCatController muscleCat)
                {
                    GanDebugger.ActorLogError("Owner is not MuscleCatController");
                    return;
                }
                _stat = muscleCat.Stat as MuscleCatStatTable;
                if (_stat == null)
                {
                    GanDebugger.ActorLogError("Stat asset is not MuscleCatStatTable");
                    return;
                }
                
                base.Owner = value;
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (Owner == null || _stat == null) return;
            
            var tr             = transform;
            var ownerTransform = Owner.transform;
            tr.position = ownerTransform.position + ownerTransform.forward * _stat.attackForwardOffset;
            tr.rotation = ownerTransform.rotation;
        }
        
        protected override void Initialize()
        {
            var player = ProjectManager.Instance.GetManager<PlayerManager>()?.GetPlayer(Define.ePlayerAvatar.MUSCLE_CAT);
            if (player == null)
            {
                GanDebugger.ActorLogError("Failed to get player");
                return;
            }
            
            base.Initialize();
            Owner = player;
            
            if (_stat == null)
            {
                GanDebugger.ActorLogError("Stat asset is not MuscleCatStatTable");
                return;
            }
            
            var tr         = transform;
            var playerTr = player.transform;
            tr.position = playerTr.position + playerTr.forward * _stat.attackForwardOffset;
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
            
            var tr             = transform;
            var attackPosition = tr.position + tr.forward * _stat.attackForwardOffset;
            var attackRadius   = _stat.skill2Radius;
            Gizmos.DrawWireSphere(attackPosition, attackRadius);
        }
#endif // UNITY_EDITOR
    }
}
