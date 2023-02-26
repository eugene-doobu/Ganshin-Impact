#nullable enable

using System;
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

        public class PlayerAvatarContext
        {
            public UIHpBarContext? RikoHpBarContext      { get; }
            public UIHpBarContext? AIHpBarContext        { get; }
            public UIHpBarContext? MuscleCatHpBarContext { get; }

            public PlayerAvatarContext()
            {
                RikoHpBarContext      = Activator.CreateInstance(typeof(UIHpBarContext)) as UIHpBarContext;
                AIHpBarContext        = Activator.CreateInstance(typeof(UIHpBarContext)) as UIHpBarContext;
                MuscleCatHpBarContext = Activator.CreateInstance(typeof(UIHpBarContext)) as UIHpBarContext;
            }
        }

#endregion Internal Class

#region Define

        private const string PlayerPoolName = "@PlayerPool";

        public struct AvatarPath
        {
            private const          string Root = "Character/Avatar";
            public static readonly string Riko = $"{Root}/Riko";
        }

        public struct AvatarBindId
        {
            public const string Riko = "PlayerManager.Riko";
        }

#endregion Define

#region Fields
        [Inject(Id = AvatarBindId.Riko)] private PlayerController _riko = null!;

        private PlayerAvatarContext _avatarContext;
        
        public Transform CurrentPlayerTransform => _riko.transform;
        
        public PlayerController CurrentPlayer => _riko;
        
        private float _maxStamina             = 100f;
        private float _currentStamina         = 100f;
        private float _staminaChargePerSecond = 10f;
        private float _staminaChargeDelay     = 1f;
        private float _currentStaminaDelay;
        
        private bool _isChargingStamina = true;
#endregion Fields

#region Properties

        public float CurrentStamina
        {
            get => _currentStamina;
            set
            {
                if (value < _currentStamina) SetStaminaDelay();
                _currentStamina = Mathf.Clamp(value, 0f, _maxStamina);
            }
        }

        private Transform _playerPool = null!;
#endregion Properties

#region Mono
        public PlayerManager()
        {
            SetPlayerPoolRoot();
            _avatarContext = new PlayerAvatarContext();
        }

        public void Initialize()
        {
            _riko.transform.SetParent(_playerPool);
            _riko.gameObject.SetActive(false);
            _riko.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
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

        public PlayerController? GetPlayer(Define.ePlayerAvatar avatar)
        {
            // TODO: 캐릭터 변경 로직으로 변경
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    return _riko;
                default:
                    return null;
            }
        }

        public UIHpBarContext? GetUIHpBarContext(Define.ePlayerAvatar avatar)
        {
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    return _avatarContext.RikoHpBarContext;
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