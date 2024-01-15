#nullable enable

using System;
using Cysharp.Threading.Tasks;
using GanShin.Content.Creature;
using GanShin.Content.Creature.Monster;
using GanShin.Data;
using GanShin.GanObject;
using UnityEngine;

namespace GanShin
{
    public class AIProjectileController : SkillObject
    {
        private readonly Collider[] _monsterColliders = new Collider[20];
        
        [SerializeField] private GameObject _impactParticle;
        [SerializeField] private GameObject _muzzleParticle;
        
        private AiStatTable? _stat;

        private Transform _tr = null!;
        private ParticleSystem[] _trails = null!;

        private Vector3 _direction;

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

        protected override void Awake()
        {
            _tr = transform;
            _trails = GetComponentsInChildren<ParticleSystem>();
            
            base.Awake();
            
            var player = ProjectManager.Instance.GetManager<PlayerManager>()?.GetPlayer(Define.ePlayerAvatar.AI);
            if (player == null)
            {
                GanDebugger.ActorLogError("Failed to get player");
                return;
            }
                       
            var playerTr        = player.transform; 
            _tr.position = playerTr.position + playerTr.forward + Vector3.up * 0.5f;

            if (_muzzleParticle == null) return;
            var muzzleParticle = Instantiate(_muzzleParticle, _tr.position, Quaternion.identity);
            muzzleParticle.transform.forward = gameObject.transform.forward;
            Destroy(muzzleParticle, 1f);
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
            
            _direction = Owner.transform.forward;
            _direction.Normalize();
            
            _tr.position = Owner.transform.position + _direction + Vector3.up * 0.5f;
            _tr.LookAt(_direction);   
            
            DestroySelf().Forget();
        }

        public override void Tick()
        {
            base.Tick();
            
            if (_stat == null) return;
            
            _tr.position += _direction * _stat.aiProjectileSpeed * Time.deltaTime;
            
            var len = Physics.OverlapSphereNonAlloc(transform.position, _stat.aiProjectileDetectRadius, _monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
            for (var i = 0; i < len; i++)
            {
                // TODO: FieldMonsterController가 아닌 MonsterController로 변경
                var monster = _monsterColliders[i].GetComponent<FieldMonsterController>();
                if (monster == null)
                    continue;

                var monsterTable = monster.Table;
            
                var monsterController = _monsterColliders[0].GetComponent<MonsterController>();
                var targetPosition = monsterController.transform.position + Vector3.up * monsterTable.monsterHeight * _stat.aiProjectileMonsterHeightRatio;
                _direction = targetPosition - transform.position;
                _direction.Normalize();
                break;
            }
        }

        private void FixedUpdate()
        {	
            if (_stat == null) return;

            var layerMask = Define.GetLayerMask(Define.eLayer.GROUND) | Define.GetLayerMask(Define.eLayer.MONSTER) | Define.GetLayerMask(Define.eLayer.ENVIRONMENT);
            var len = Physics.OverlapSphereNonAlloc(transform.position, _stat.aiProjectileRadius, _monsterColliders, layerMask);
            if (len <= 0) 
                return;
            
            Impact();
        }

        private async UniTask DestroySelf()
        {
            if (_stat == null)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_stat.aiProjectileDuration), cancellationToken: destroyCancellationToken);

            Impact();
        }

        private void Impact()
        {
            var impactParticleObject = Instantiate(_impactParticle, _tr.position, _tr.rotation);

            for (var i = 1; i < _trails.Length; i++)
            {
                var trail = _trails[i];

                if (!trail.gameObject.name.Contains("Trail")) 
                    continue;
                    
                trail.transform.SetParent(null);
                Destroy(trail.gameObject, 2f);
            }

            if (_stat != null)
            {
                var len = Physics.OverlapSphereNonAlloc(transform.position, _stat.aiProjectileRadius, _monsterColliders, Define.GetLayerMask(Define.eLayer.MONSTER));
                for (var i = 0; i < len; i++)
                {
                    var monster = _monsterColliders[i].GetComponent<MonsterController>();
                    if (monster == null)
                        continue;
                
                    monster.OnDamaged(_stat.aiProjectileDamage);
                }
            }

            Destroy(impactParticleObject, 3.5f);
            Destroy(gameObject);
        }
    }
}
