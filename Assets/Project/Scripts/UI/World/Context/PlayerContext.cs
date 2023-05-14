using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI
{
    /// <summary>
    /// 플레이어의 아바타 상관없이 적용되는 수치에 대한 viewmodel context
    /// </summary>
    public class PlayerContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
#region Fields
        private float _currentCurrentStamina;
        private float _maxStamina;
        private float _staminaPercent;
#endregion Fields

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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
