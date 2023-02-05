using System.ComponentModel;

namespace GanShin.UI
{
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        public LoadingSceneDataContext LoadingSceneDataContext => 
            DataContext as LoadingSceneDataContext;
        
        protected override INotifyPropertyChanged InitializeDataContext() =>
            new LoadingSceneDataContext();

        public override void ClearContextData()
        {
            var context = LoadingSceneDataContext;
            context.LoadingText = string.Empty;
            context.Progress    = 0;
        }
    }
}
