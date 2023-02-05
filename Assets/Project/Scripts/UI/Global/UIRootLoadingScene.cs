using System.ComponentModel;
using UnityEngine;

namespace GanShin.UI
{
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        public LoadingSceneDataContext LoadingSceneDataContext => 
            DataContext as LoadingSceneDataContext;
        
        protected override INotifyPropertyChanged InitializeDataContext() =>
            new LoadingSceneDataContext();

        public void Update()
        {
            if(Input.GetKeyDown("]"))
            {
                LoadingSceneDataContext.Progress += 0.1f;
            }
            else if (Input.GetKeyDown("["))
            {
                LoadingSceneDataContext.Progress -= 0.1f;
            }
        }

        public override void ClearContextData()
        {
            var context = LoadingSceneDataContext;
            context.LoadingText = string.Empty;
            context.Progress    = 0;
        }
    }
}
