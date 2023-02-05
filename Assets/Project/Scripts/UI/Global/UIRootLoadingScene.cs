using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GanShin.UI
{
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        [Inject(Id = LoadingSceneInstaller.TipsId)] 
        private List<string> _tips;
        
        [Inject(Id = LoadingSceneInstaller.ProgressSmoothFactorId)] 
        private float _progressSmoothFactor;
        
        public LoadingSceneDataContext LoadingSceneDataContext => 
            DataContext as LoadingSceneDataContext;
        
        protected override INotifyPropertyChanged InitializeDataContext() =>
            new LoadingSceneDataContext();
        
        private bool  _isInitialized;
        private float _targetProgress;
        private float _viewProgress;

        private void Update()
        {
            if (!_isInitialized) return;
            _viewProgress = Mathf.Lerp(_viewProgress, _targetProgress,
                _progressSmoothFactor * Time.deltaTime);
            LoadingSceneDataContext.Progress = _viewProgress;
        }

        public void SetProgress(float progress)
        {
            _targetProgress = progress;
        }
        
        public override void InitializeContextData()
        {
            _isInitialized = true;
            if (_tips == null || _tips.Count == 0)
                LoadingSceneDataContext.LoadingText = string.Empty;
            else
                LoadingSceneDataContext.LoadingText = _tips[Random.Range(0, _tips.Count)];
        }

        public override void ClearContextData()
        {
            _isInitialized = false;
            var context = LoadingSceneDataContext;
            context.LoadingText = string.Empty;
            context.Progress    = 0;
            _targetProgress     = 0f;
            _viewProgress       = 0f;
        }
    }
}
