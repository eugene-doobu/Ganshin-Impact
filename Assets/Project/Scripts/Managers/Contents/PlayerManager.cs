#nullable enable

using System;
using GanShin.CameraSystem;
using GanShin.Content.Creature;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GanShin
{
    [UsedImplicitly]
    public class PlayerManager : IInitializable, ITickable
    {
#region Internal Class
        private class PlayerAvatarContextBundle
        {
            public PlayerAvatarContext? RikoHpBarContext      { get; }
            public PlayerAvatarContext? AIHpBarContext        { get; }
            public PlayerAvatarContext? MuscleCatHpBarContext { get; }

            public PlayerAvatarContextBundle()
            {
                RikoHpBarContext      = Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
                AIHpBarContext        = Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
                MuscleCatHpBarContext = Activator.CreateInstance(typeof(PlayerAvatarContext)) as PlayerAvatarContext;
            }
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
        [Inject(Id = AvatarBindId.Riko)]      private RikoController      _riko      = null!;
        [Inject(Id = AvatarBindId.Ai)]        private AiController        _ai        = null!;
        [Inject(Id = AvatarBindId.MuscleCat)] private MuscleCatController _muscleCat = null!;

        [Inject]
        CameraManager _camera = null!;
        
        private readonly PlayerAvatarContextBundle _avatarContextBundle = new PlayerAvatarContextBundle();
        
        private readonly PlayerContext? _playerContext = Activator.CreateInstance(typeof(PlayerContext)) as PlayerContext;

        public Transform CurrentPlayerTransform => CurrentPlayer.transform;
        
        public PlayerController CurrentPlayer
        {
            get
            {
                return _currentAvatar switch
                {
                    Define.ePlayerAvatar.RIKO       => _riko,
                    Define.ePlayerAvatar.AI         => _ai,
                    Define.ePlayerAvatar.MUSCLE_CAT => _muscleCat,
                    _                               => throw new ArgumentOutOfRangeException()
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
        
        private Define.ePlayerAvatar _currentAvatar = Define.ePlayerAvatar.RIKO;
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

        public void Initialize()
        {
            InitializeCharacter(_riko);
            InitializeCharacter(_ai);
            InitializeCharacter(_muscleCat);
        }

        private void InitializeCharacter(PlayerController character)
        {
            character.transform.SetParent(_playerPool);
            character.gameObject.SetActive(false);
            character.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void Tick()
        {
            ChargeStaminaDelay();
            ChargeStamina();
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

        public PlayerController? SetCurrentPlayer(Define.ePlayerAvatar avatar)
        {
            if (_currentAvatar == avatar) 
                return GetPlayer(avatar);
            
            var player = ActivePlayerContext(avatar);
            if (player == null) return null;
            
            _camera.ChangeTarget(player.transform);
            var prevPlayer = ActivePlayerContext(_currentAvatar, false);
            var prevTr = prevPlayer.transform;
            
            // 포지션 변경의 문제가 있어서 변경될 오브젝트를 비활성화 후 활성화
            player.gameObject.SetActive(false);
            player.transform.SetPositionAndRotation(prevTr.position, prevTr.rotation);
            player.gameObject.SetActive(true);

            _onPlayerChanged?.Invoke(player);
            
            _currentAvatar = avatar;
            return player;
        }

        private PlayerController? ActivePlayerContext(Define.ePlayerAvatar avatar, bool value = true)
        {
            var player = GetPlayer(avatar);
            if (player == null) return null;

            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    _playerContext.IsRikoActive = value;
                    _avatarContextBundle.RikoHpBarContext.IsActive = value;
                    player.gameObject.SetActive(value);
                    break;
                case Define.ePlayerAvatar.AI:
                    _playerContext.IsAiActive = value;
                    player.gameObject.SetActive(value);
                    _avatarContextBundle.AIHpBarContext.IsActive = value;
                    break;
                case Define.ePlayerAvatar.MUSCLE_CAT:
                    _playerContext.IsMuscleCatActive = value;
                    player.gameObject.SetActive(value);
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