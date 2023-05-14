using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI
{
    public sealed class PlayerAvatarContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _targetName;
        private int    _currentHp;
        private int    _maxHp;
        private float  _hpPercent;

        [UsedImplicitly]
        public string TargetName
        {
            get => _targetName;
            set
            {
                _targetName = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public int CurrentHp
        {
            get => _currentHp;
            set
            {
                _currentHp = value;
                OnPropertyChanged();
                HpPercent = (float) _currentHp / _maxHp;
            }
        }

        [UsedImplicitly]
        public int MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public float HpPercent
        {
            get => _hpPercent;
            set
            {
                _hpPercent = Mathf.Clamp(value, 0, 1f);
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