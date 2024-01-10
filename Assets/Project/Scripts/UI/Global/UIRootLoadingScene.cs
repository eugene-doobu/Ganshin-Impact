using System.Collections.Generic;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI
{
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        private bool _isInitialized;

        // TODO: Addressable로 변경
        //[Inject(Id = LoadingSettingInstaller.ProgressSmoothFactorId)]
        private float _progressSmoothFactor;

        private float _targetProgress;

        // TODO: Addressable로 변경
        //[Inject(Id = LoadingSettingInstaller.TipsId)]
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