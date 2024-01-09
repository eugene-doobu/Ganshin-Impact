using GanShin.Space.Content;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GanShin.Space.UI
{
    public class UIMiniMap : UIRootBase
    {
        /// <summary>
        ///     Texture가 null일 경우 사용되는 기본 텍스쳐
        /// </summary>
        [SerializeField] private Texture defaultTexture;

        [SerializeField] private RawImage       rawImage;
        private readonly         MinimapManager _minimapManager = ProjectManager.Instance.GetManager<MinimapManager>();

        protected override Context InitializeDataContext()
        {
            var context = new MinimapContext
            {
                DefaultTexture = defaultTexture,
                Texture        = _minimapManager?.GetMinimapTexture()
            };
            return context;
        }
    }
}