using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    [UsedImplicitly]
    public class LoadingSceneContext : Context
    {
        private readonly Property<string> _loadingText = new();
        private readonly Property<float>  _progress    = new();

        public string LoadingText
        {
            get => _loadingText.Value;
            set => _loadingText.Value = value;
        }

        public float Progress
        {
            get => _progress.Value;
            set => _progress.Value = value;
        }
    }
}