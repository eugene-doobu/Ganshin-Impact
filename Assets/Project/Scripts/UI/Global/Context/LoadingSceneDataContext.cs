using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI
{
    public sealed class LoadingSceneDataContext : GanContext
    {
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
    }
}