using GanShin.Space.Content;
using GanShin.UI;
using UnityEngine;
using UnityEngine.UI;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Space.UI
{
    public class UIMiniMap : UIRootBase
    {
        private MinimapManager _minimapManager = ProjectManager.Instance.GetManager<MinimapManager>();
        
        /// <summary>
        /// Texture가 null일 경우 사용되는 기본 텍스쳐
        /// </summary>
        [SerializeField] private Texture defaultTexture;
        [SerializeField] private RawImage rawImage;

        protected override Context InitializeDataContext()
        {
            var context = new MinimapContext
            {
                DefaultTexture = defaultTexture,
                Texture = _minimapManager?.GetMinimapTexture(),
            };
            return context;
        }
    }
}
