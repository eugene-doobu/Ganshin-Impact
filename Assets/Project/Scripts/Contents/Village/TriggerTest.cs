using GanShin.GanObject;
using GanShin.Space.UI;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace GanShin.Village.Base
{
    public class TriggerTest : MonoBehaviour, ITriggerEventProvider
    {
        [Inject] UIManager _ui;
        
        private InteractionItemContext _itemContext;
        
        public void OnTriggerEnter(Collider other)
        {
            if (_itemContext != null)
                return;
            
            var context = _ui.GetOrAddContext<InteractionContext>();
            _itemContext = new InteractionItemContext
            {
                Name = "Test",
            };
            _itemContext.OnClickEvent += () =>
            {
                _ui.AddLog("Npc와 인터렉션을 시도하였습니다.");
            };
            context.Items.Add(_itemContext);
            
            Debug.Log("OnTriggerEnter");
        }

        public void OnTriggerExit(Collider other)
        {
            if(_itemContext == null)
                return;
            
            var context = _ui.GetContext<InteractionContext>();
            context?.Items.Remove(_itemContext);
            _itemContext.Dispose();
            _itemContext = null;
            
            Debug.Log("OnTriggerExit");
        }
    }
}
