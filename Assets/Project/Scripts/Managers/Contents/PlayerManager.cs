#nullable enable

using System;
using GanShin.CameraSystem;
using GanShin.Content.Creature;
using GanShin.Space.UI;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GanShin
{
    [UsedImplicitly]
    public class PlayerManager : ManagerBase
    {
#region Internal Class
        private class PlayerAvatarContextBundle
        {
            public PlayerAvatarContext? RikoHpBarContext      { get; } = Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
            public PlayerAvatarContext? AIHpBarContext        { get; } = Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
            public PlayerAvatarContext? MuscleCatHpBarContext { get; } = Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
        }
#endregion Internal Class

#region Define
        private const string PlayerPoolName = "@PlayerPool";

        public struct AvatarPath
        {
            private const          string Root      = "Character/Avatar";
            public static readonly string Riko      = $"{Root}/Riko";
            public static readonly string Ai        = $"{Root}/Ai";
            public static readonly string MuscleCat = $"{Root}/MuscleCat";
        }

        public struct AvatarBindId
        {
            public const string Riko      = "PlayerManager.Riko";
            public const string Ai        = "PlayerManager.Ai";
            public const string MuscleCat = "PlayerManager.MuscleCat";
        }
#endregion Define

#region Fields
        private RikoController      _riko      = null!;
        private AiController        _ai        = null!;
        private MuscleCatController _muscleCat = null!;

        CameraManager _camera = ProjectManager.Instance.GetManager<CameraManager>()!;
        
        private readonly PlayerAvatarContextBundle _avatarContextBundle = new PlayerAvatarContextBundle();
        
        private readonly PlayerContext? _playerContext = Activator.CreateInstance(typeof(PlayerContext)) as PlayerContext;

        public Transform? CurrentPlayerTransform => _currentAvatar == Define.ePlayerAvatar.NONE ? null : CurrentPlayer!.transform;
        
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
        
        private float _maxStamina             = 100f;
        private float _currentStamina         = 100f;
        private float _staminaChargePerSecond = 10f;
        private float _staminaChargeDelay     = 1f;
        private float _currentStaminaDelay;
        
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
                _currentStamina = Mathf.Clamp(value, 0f, _maxStamina);
                _playerContext.CurrentStamina = _currentStamina;
            }
        }

        private Transform _playerPool = null!;
#endregion Properties

#region Mono
        public PlayerManager()
        {
            SetPlayerPoolRoot();
            _playerContext.MaxStamina = _maxStamina;
        }

        public override void Initialize()
        {
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

        private void InstallCharacters()
        {
            _riko      = Object.Instantiate(Resources.Load<RikoController>(AvatarPath.Riko));
            _ai        = Object.Instantiate(Resources.Load<AiController>(AvatarPath.Ai));
            _muscleCat = Object.Instantiate(Resources.Load<MuscleCatController>(AvatarPath.MuscleCat));
        }
        
        public PlayerController? SetCurrentPlayer(Define.ePlayerAvatar avatar)
        {
            if (_currentAvatar == avatar) 
                return GetPlayer(avatar);
            
            var player = ActivePlayerContext(avatar);
            if (player == null) return null;
            if (player.CurrentHp <= 0) return null;
            
            _camera.ChangeTarget(player.transform);
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
            return player;
        }

        private PlayerController? ActivePlayerContext(Define.ePlayerAvatar avatar, bool value = true)
        {
            var player = GetPlayer(avatar);
            if (player == null) return null;
            
            var isDead = player.CurrentHp <= 0;
            player.gameObject.SetActive(value);

            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    _playerContext.IsRikoActive                    = !isDead && value;
                    _avatarContextBundle.RikoHpBarContext.IsActive = value;
                    break;
                case Define.ePlayerAvatar.AI:
                    _playerContext.IsAiActive                    = !isDead && value;
                    _avatarContextBundle.AIHpBarContext.IsActive = value;
                    break;
                case Define.ePlayerAvatar.MUSCLE_CAT:
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
            var root = GameObject.Find(PlayerPoolName);
            if (root != null) return;
            root = new GameObject {name = PlayerPoolName};
            Object.DontDestroyOnLoad(root);

            _playerPool = root.transform;
        }
    }
}