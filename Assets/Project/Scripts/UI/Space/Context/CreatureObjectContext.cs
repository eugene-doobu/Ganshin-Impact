using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Space.UI
{
    public abstract class CreatureObjectContext : GanContext
    {
#region Fields
        private bool   _isEnable;
        private string _targetName;
        private int    _currentHp;
        private int    _maxHp;
        private float  _hpPercent;
        private float  _sortOrder;
#endregion Fields

#region Field Properties
        [UsedImplicitly]
        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                OnPropertyChanged();
            }
        }
        
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
        
        [UsedImplicitly]
        public float SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = value;
                OnPropertyChanged();
            }
        }
#endregion Field Properties
    }
}