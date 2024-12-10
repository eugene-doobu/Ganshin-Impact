using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI.Space
{
    /// <summary>
    ///     플레이어의 아바타 상관없이 적용되는 수치에 대한 viewmodel context
    /// </summary>
    public class PlayerContext : GanContext
    {
        [UsedImplicitly]
        public bool IsRikoActive
        {
            get => _isRikoActive;
            set
            {
                _isRikoActive = value;
                OnPropertyChanged();

                if (!value) return;
                IsAiActive        = false;
                IsMuscleCatActive = false;
            }
        }

        [UsedImplicitly]
        public bool IsAiActive
        {
            get => _isAiActive;
            set
            {
                _isAiActive = value;
                OnPropertyChanged();

                if (!value) return;
                IsRikoActive      = false;
                IsMuscleCatActive = false;
            }
        }

        [UsedImplicitly]
        public bool IsMuscleCatActive
        {
            get => _isMuscleCatActive;
            set
            {
                _isMuscleCatActive = value;
                OnPropertyChanged();

                if (!value) return;
                IsRikoActive = false;
                IsAiActive   = false;
            }
        }

        [UsedImplicitly]
        public float CurrentStamina
        {
            get => _currentCurrentStamina;
            set
            {
                _currentCurrentStamina = value;
                OnPropertyChanged();
                StaminaPercent = _currentCurrentStamina / _maxStamina;
            }
        }

        [UsedImplicitly]
        public float MaxStamina
        {
            get => _maxStamina;
            set
            {
                _maxStamina = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public float StaminaPercent
        {
            get => _staminaPercent;
            set
            {
                _staminaPercent = Mathf.Clamp(value, 0, 1f);
                OnPropertyChanged();
            }
        }

#region Fields

        private bool _isRikoActive;
        private bool _isAiActive;
        private bool _isMuscleCatActive;

        private float _currentCurrentStamina;
        private float _maxStamina;
        private float _staminaPercent;

#endregion Fields
    }
}