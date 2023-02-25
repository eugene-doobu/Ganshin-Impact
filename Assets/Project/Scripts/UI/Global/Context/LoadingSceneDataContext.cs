using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI
{
    public sealed class LoadingSceneDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private string _loadingText;
        private float  _progress;
        
        [UsedImplicitly]
        public string LoadingText
        {
            get => _loadingText;
            set
            {
                _loadingText = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = Mathf.Clamp(value, 0, 1f);
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
