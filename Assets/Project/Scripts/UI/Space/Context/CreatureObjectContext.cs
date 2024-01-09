using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Space.UI
{
    public abstract class CreatureObjectContext : GanContext
    {
#region Fields

        private string _targetName;
        private int    _currentHp;
        private int    _maxHp;
        private float  _hpPercent;

#endregion Fields

#region Field Properties

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
                HpPercent = (float)_currentHp / _maxHp;
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

#endregion Field Properties
    }
}