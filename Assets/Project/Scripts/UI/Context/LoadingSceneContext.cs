using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class LoadingSceneContext : Context
    {
        private readonly Property<string> _loadingText = new Property<string>();
        private readonly Property<float>  _progress    = new Property<float>();

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