#nullable enable

using System;
using GanShin.CameraSystem;
using GanShin.Content.Creature;
using GanShin.Resource;
using GanShin.UI.Space;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GanShin
{
    [UsedImplicitly]
    public class PlayerManager : ManagerBase
    {
#region Define
        private const string PLAYER_POOL_NAME = "@PlayerPool";
#endregion Define

        [UsedImplicitly]
        public PlayerManager()
        {
        }

        private void InstallCharacters()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>()!;

            var rikoObject = resourceManager.Instantiate("Riko.prefab");
            if (rikoObject != null)
                _riko = rikoObject.GetComponent<RikoController>()!;

            var aiObject = resourceManager.Instantiate("Ai.prefab");
            if (aiObject != null)
                _ai = aiObject.GetComponent<AiController>()!;

            var muscleCatObject = resourceManager.Instantiate("MuscleCat.prefab");
            if (muscleCatObject != null)
                _muscleCat = muscleCatObject.GetComponent<MuscleCatController>()!;
        }

        public PlayerController? SetCurrentPlayer(Define.ePlayerAvatar avatar)
        {
            if (_currentAvatar == avatar)
            {
                var currentPlayer = GetPlayer(avatar);
                if (currentPlayer == null) 
                    return null;
                
                SetCullingGroupPlayer(currentPlayer.transform);
                return currentPlayer;
            }

            var player = ActivePlayerContext(avatar);
            if (player == null) return null;
            if (player.CurrentHp <= 0) return null;

            var camera = ProjectManager.Instance.GetManager<CameraManager>()!;
            camera.ChangeTarget(player.transform);
            var prevPlayer = ActivePlayerContext(_currentAvatar, false);
            if (prevPlayer != null)
            {
                var prevTr = prevPlayer.transform;

                // 포지션 변경의 문제가 있어서 변경될 오브젝트를 비활성화 후 활성화
                player.gameObject.SetActive(false);
                player.transform.SetPositionAndRotation(prevTr.position, prevTr.rotation);
                player.gameObject.SetActive(true);
            }

            _onPlayerChanged?.Invoke(player);

            _currentAvatar = avatar;
            
            SetCullingGroupPlayer(player.transform);
            
            return player;
        }

        private void SetCullingGroupPlayer(Transform playerTransform)
        {
            var cameraManager = ProjectManager.Instance.GetManager<CameraManager>();
            var cullingGroup  = cameraManager?.GetOrAddCullingGroupProxy(eCullingGroupType.OBJECT_HUD);
            if (!ReferenceEquals(cullingGroup, null))
            {
                cullingGroup.DistanceReferencePoint = playerTransform;
            }
        }

        private PlayerController? ActivePlayerContext(Define.ePlayerAvatar avatar, bool value = true)
        {
            var player = GetPlayer(avatar);
            if (player == null) return null;

            var isDead = player.CurrentHp <= 0;
            player.gameObject.SetActive(value);
            
            if (_playerContext == null) return player;

            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    if (_avatarContextBundle.RikoHpBarContext == null) return player;
                    _playerContext.IsRikoActive                    = !isDead && value;
                    _avatarContextBundle.RikoHpBarContext.IsActive = value;
                    break;
                case Define.ePlayerAvatar.AI:
                    if (_avatarContextBundle.AIHpBarContext == null) return player;
                    _playerContext.IsAiActive                    = !isDead && value;
                    _avatarContextBundle.AIHpBarContext.IsActive = value;
                    break;
                case Define.ePlayerAvatar.MUSCLE_CAT:
                    if (_avatarContextBundle.MuscleCatHpBarContext == null) return player;
                    _playerContext.IsMuscleCatActive                    = !isDead && value;
                    _avatarContextBundle.MuscleCatHpBarContext.IsActive = value;
                    break;
            }

            return player;
        }

        public PlayerController? GetPlayer(Define.ePlayerAvatar avatar)
        {
            // TODO: 캐릭터 변경 로직으로 변경
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    return _riko;
                case Define.ePlayerAvatar.AI:
                    return _ai;
                case Define.ePlayerAvatar.MUSCLE_CAT:
                    return _muscleCat;
                default:
                    return null;
            }
        }

        public PlayerAvatarContext? GetAvatarContext(Define.ePlayerAvatar avatar)
        {
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    return _avatarContextBundle.RikoHpBarContext;
                case Define.ePlayerAvatar.AI:
                    return _avatarContextBundle.AIHpBarContext;
                case Define.ePlayerAvatar.MUSCLE_CAT:
                    return _avatarContextBundle.MuscleCatHpBarContext;
                default:
                    return null;
            }
        }

        private void SetPlayerPoolRoot()
        {
            var root = GameObject.Find(PLAYER_POOL_NAME);
            if (root != null) return;
            root = new GameObject { name = PLAYER_POOL_NAME };
            Object.DontDestroyOnLoad(root);

            _playerPool = root.transform;
        }

#region Internal Class
        private class PlayerAvatarContextBundle
        {
            public PlayerAvatarContext? RikoHpBarContext { get; } =
                Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;

            public PlayerAvatarContext? AIHpBarContext { get; } =
                Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;

            public PlayerAvatarContext? MuscleCatHpBarContext { get; } =
                Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
        }
#endregion Internal Class

#region Fields
        private RikoController      _riko      = null!;
        private AiController        _ai        = null!;
        private MuscleCatController _muscleCat = null!;

        private readonly PlayerAvatarContextBundle _avatarContextBundle = new();

        private readonly PlayerContext? _playerContext =
            Activator.CreateInstance(typeof(PlayerContext)) as PlayerContext;

        public Transform? CurrentPlayerTransform
        {
            get
            {
                if (CurrentPlayer == null) return null;
                return _currentAvatar == Define.ePlayerAvatar.NONE ? null : CurrentPlayer!.transform;
            }
        }

        public PlayerController? CurrentPlayer
        {
            get
            {
                return _currentAvatar switch
                {
                    Define.ePlayerAvatar.RIKO       => _riko,
                    Define.ePlayerAvatar.AI         => _ai,
                    Define.ePlayerAvatar.MUSCLE_CAT => _muscleCat,
                    _                               => null
                };
            }
        }

        public PlayerContext PlayerContext => _playerContext!;

        private readonly float _maxStamina             = 100f;
        private          float _currentStamina         = 100f;
        private readonly float _staminaChargePerSecond = 10f;
        private readonly float _staminaChargeDelay     = 1f;
        private          float _currentStaminaDelay;

        private bool _isChargingStamina = true;

        private Define.ePlayerAvatar _currentAvatar = Define.ePlayerAvatar.NONE;
#endregion Fields

#region Event
        private Action<PlayerController>? _onPlayerChanged;

        public event Action<PlayerController>? OnPlayerChanged
        {
            add
            {
                _onPlayerChanged -= value;
                _onPlayerChanged += value;
            }
            remove => _onPlayerChanged -= value;
        }
#endregion Event

#region Properties
        public float CurrentStamina
        {
            get => _currentStamina;
            set
            {
                if (value < _currentStamina) SetStaminaDelay();
                _currentStamina               = Mathf.Clamp(value, 0f, _maxStamina);
                if (_playerContext != null)
                    _playerContext.CurrentStamina = _currentStamina;
            }
        }

        private Transform _playerPool = null!;
#endregion Properties

#region Mono
        public override void PostInitialize()
        {
            SetPlayerPoolRoot();
            if (_playerContext != null)
                _playerContext.MaxStamina = _maxStamina;

            InstallCharacters();
            InitializeCharacter(_riko);
            InitializeCharacter(_ai);
            InitializeCharacter(_muscleCat);
        }

        public override void Tick()
        {
            ChargeStaminaDelay();
            ChargeStamina();
        }

        private void InitializeCharacter(PlayerController character)
        {
            character.transform.SetParent(_playerPool);
            character.gameObject.SetActive(false);
            character.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
#endregion Mono

#region Stamina
        private void ChargeStamina()
        {
            if (!_isChargingStamina) return;
            CurrentStamina += _staminaChargePerSecond * Time.deltaTime;
        }

        private void SetStaminaDelay()
        {
            _currentStaminaDelay = _staminaChargeDelay;
            _isChargingStamina   = false;
        }

        private void ChargeStaminaDelay()
        {
            if (_isChargingStamina) return;

            _currentStaminaDelay -= Time.deltaTime;
            if (_currentStaminaDelay > 0) return;
            _isChargingStamina = true;
        }
#endregion Stamina
    }
}