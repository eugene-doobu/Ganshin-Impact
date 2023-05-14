using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI
{
    public sealed class PlayerAvatarContext : CreatureObjectContext
    {
#region Fields
        private bool _isDead;
        
        private float _currentBaseSkillCoolTime;
        private float _baseSkillCoolTime;
        private float _baseSkillCoolTimePercent;
        
        private float _currentUltimateGauge;
        private float _ultimateGauge;
        private float _ultimateGaugePercent;
#endregion Fields

#region Field Properties
        [UsedImplicitly]
        public bool IsDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float CurrentBaseSkillCoolTime
        {
            get => _currentBaseSkillCoolTime;
            set
            {
                _currentBaseSkillCoolTime = value;
                OnPropertyChanged();
                BaseSkillCoolTimePercent = _currentBaseSkillCoolTime / _baseSkillCoolTime;
            }
        }
        
        [UsedImplicitly]
        public float BaseSkillCoolTime
        {
            get => _baseSkillCoolTime;
            set
            {
                _baseSkillCoolTime = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float BaseSkillCoolTimePercent
        {
            get => _baseSkillCoolTimePercent;
            set
            {
                _baseSkillCoolTimePercent = Mathf.Clamp(value, 0, 1f);
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float CurrentUltimateGauge
        {
            get => _currentUltimateGauge;
            set
            {
                _currentUltimateGauge = value;
                OnPropertyChanged();
                UltimateGaugePercent = _currentUltimateGauge / _ultimateGauge;
            }
        }
        
        [UsedImplicitly]
        public float UltimateGauge
        {
            get => _ultimateGauge;
            set
            {
                _ultimateGauge = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float UltimateGaugePercent
        {
            get => _ultimateGaugePercent;
            set
            {
                _ultimateGaugePercent = Mathf.Clamp(value, 0, 1f);
                OnPropertyChanged();
            }
        }
#endregion Field Properties
    }
}