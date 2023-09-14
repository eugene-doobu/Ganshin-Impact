using Cysharp.Threading.Tasks;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GanShin.Space.UI
{
    public class UILog : UIRootBase
    {        
        [SerializeField] private RectTransform[] layoutRoots = null!;

        protected override Context InitializeDataContext()
        {
            var context = UIManager.GetOrAddContext<LogContext>();
            context.Items.ItemAdded -= OnItemAdded;
            context.Items.ItemAdded += OnItemAdded;
            return context;
        }

        private void OnItemAdded(object item)
        {
            RefreshLayout(item).Forget();
        }

        private async UniTask RefreshLayout(object item)
        {
            await UniTask.NextFrame();

            if (item is RectTransform rectTransform)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

            foreach (var layoutRoot in layoutRoots)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
        }
    }
}
