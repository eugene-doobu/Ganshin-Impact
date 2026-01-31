using GanShin.GanObject;
using GanShin.UI.Space;
using GanShin.UI;
using UnityEngine;

namespace GanShin.Village.Base
{
    public class TriggerTest : MonoBehaviour, ITriggerEventProvider
    {
        private InteractionItemContext _itemContext;
        private UIManager              UI => ProjectManager.Instance.GetManager<UIManager>();

        public void OnTriggerEnter(Collider other)
        {
            if (_itemContext != null)
                return;

            var context = UI.GetOrAddContext<InteractionContext>();
            _itemContext = new InteractionItemContext
            {
                Name = "Test"
            };
            _itemContext.OnClickEvent += () => { UI.AddLog("Npc와 인터렉션을 시도하였습니다."); };
            context.Items.Add(_itemContext);

            Debug.Log("OnTriggerEnter");
        }

        public void OnTriggerExit(Collider other)
        {
            if (_itemContext == null)
                return;

            var context = UI.GetContext<InteractionContext>();
            context?.Items.Remove(_itemContext);
            _itemContext.Dispose();
            _itemContext = null;

            Debug.Log("OnTriggerExit");
        }
    }
}