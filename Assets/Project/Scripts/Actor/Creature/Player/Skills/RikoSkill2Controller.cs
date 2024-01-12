#nullable enable

using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using GanShin.Content.Creature;
using GanShin.Data;
using GanShin.Effect;
using UnityEngine;

namespace GanShin.GanObject
{
    public class RikoSkill2Controller : SkillObject
    {
        [SerializeField] private ParticleSystem           _particleObject = null!;
        [SerializeField] private CinemachineImpulseSource _impulseSource  = null!;

        private RikoStatTable? _stat;
        
        private readonly Collider[] _monsterColliders = new Collider[25];

        public override CreatureObject Owner
        {
            get => base.Owner;
            set
            {
                if (value is not RikoController riko)
                {
                    GanDebugger.ActorLogError("Owner is not RikoController");
                    return;
                }
                _stat = riko.Stat as RikoStatTable;
                if (_stat == null)
                {
                    GanDebugger.ActorLogError("Stat asset is not RikoStatTable");
                    return;
                }
                
                base.Owner = value;
            }
        }
        
        protected override void Initialize()
        {
            var player = ProjectManager.Instance.GetManager<PlayerManager>()?.GetPlayer(Define.ePlayerAvatar.RIKO);
            if (player == null)
            {
                GanDebugger.ActorLogError("Failed to get player");
                return;
            }
            
            base.Initialize();
            Owner = player;
                
            StartParticle().Forget();
        }

        private async UniTask StartParticle()
        {
            if (_stat == null)
            {
                GanDebugger.ActorLogError("Stat is null");
                return;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_stat.skill2Delay));
            _particleObject.Play();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_stat.skill2DamageDelay));

            if (Owner is not PlayerController player)
            {
                GanDebugger.ActorLogError("Owner is not PlayerController");
                return;
            }
            
            // TODO: Sound
            
            player.ApplyAttackDamageCapsule(transform.position, transform.forward, _stat.skill2CapsuleRadius, _stat.skill2CapsuleHeight,
                _stat.skill2Damage, _monsterColliders, OnSkillEffect);
            
            _impulseSource.GenerateImpulseWithForce(_stat.skill2ShakeForce);
        }

        private void OnSkillEffect(Collider monsterCollider)
        {
            var effect = ProjectManager.Instance.GetManager<EffectManager>();
            if (effect == null)
            {
                GanDebugger.ActorLogWarning("Failed to get EffectManager");
                return;
            }
            
            var closetPoint = monsterCollider.ClosestPoint(transform.position);
            effect.PlayEffect(eEffectType.RIKO_SWORD_ULTIMATE_HIT, closetPoint);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stat == null)
                return;
            
            DebuggingUtil.DrawCapsule(transform.position, transform.forward, _stat.skill2CapsuleRadius, _stat.skill2CapsuleHeight, Color.red);
        }
#endif // UNITY_EDITOR
    }
}
