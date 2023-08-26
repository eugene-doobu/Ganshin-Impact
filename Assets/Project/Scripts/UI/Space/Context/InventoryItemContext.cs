using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.Space.UI
{
    public class InventoryItemContext : Context, INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _amount;
        private float _coolTime;
        
        [UsedImplicitly]
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float CoolTime
        {
            get => _coolTime;
            set
            {
                _coolTime = value;
                OnPropertyChanged();
            }
        }

        public void Dispose()
        {
            Amount          = 0;
            CoolTime        = 1f;
            PropertyChanged = null;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}