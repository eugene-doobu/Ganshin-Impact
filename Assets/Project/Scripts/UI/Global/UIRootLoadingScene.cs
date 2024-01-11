using System.Collections.Generic;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI
{
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        private bool _isInitialized;

        private float _progressSmoothFactor;
        private float _targetProgress;

        private List<string> _tips;
        private float        _viewProgress;

        public LoadingSceneDataContext LoadingSceneDataContext =>
            DataContext as LoadingSceneDataContext;

        private void Update()
        {
            if (!_isInitialized) return;
            _viewProgress = Mathf.Lerp(_viewProgress, _targetProgress,
                                       _progressSmoothFactor * Time.deltaTime);
            LoadingSceneDataContext.Progress = _viewProgress;
        }

        protected override Context InitializeDataContext()
        {
            var loadingSetting = Util.LoadAsset<LoadingSettingInstaller>("LoadingSceneSetting.asset");
            if (loadingSetting != null)
            {
                _tips                 = loadingSetting.tips;
                _progressSmoothFactor = loadingSetting.progressSmoothFactor;
            }
            
            return new LoadingSceneDataContext();
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