using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace GanShin.UI
{
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        [Inject(Id = LoadingSceneInstaller.TipsId)] 
        private List<string> _tips;
        
        public LoadingSceneDataContext LoadingSceneDataContext => 
            DataContext as LoadingSceneDataContext;
        
        protected override INotifyPropertyChanged InitializeDataContext() =>
            new LoadingSceneDataContext();

        private void OnEnable()
        {
            if (_tips == null || _tips.Count == 0)
                LoadingSceneDataContext.LoadingText = string.Empty;
            else
                LoadingSceneDataContext.LoadingText = _tips[Random.Range(0, _tips.Count)];
            LoadingSceneDataContext.Progress    = 0f;
        }

        public override void ClearContextData()
        {
            var context = LoadingSceneDataContext;
            context.LoadingText = string.Empty;
            context.Progress    = 0;
        }
    }
}
