using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GanShin.UI
{
    public enum eGlobalUI
    {
        LOADING,
    }
    
    public partial class UIManager
    {
#region Define
        public struct GlobalUIName
        {
            private const string Root    = "Prefabs/UI/Global/";
            public static readonly string Loading = $"{Root}UI_LoadingScene";
        }
#endregion Define
        
        private readonly Dictionary<eGlobalUI, GlobalUIRootBase> _globalUIs = new ();
        public GlobalUIRootBase GetGlobalUI(eGlobalUI ui) => 
            _globalUIs.ContainsKey(ui) ? _globalUIs[ui] : null;
            
        public void OnGlobalUI(eGlobalUI ui, bool isOn)
        {
            if (isOn)
            {
                if (_globalUIs.ContainsKey(ui))
                    _globalUIs[ui].gameObject.SetActive(true);
            }
            else
            {
                if (_globalUIs.ContainsKey(ui))
                    _globalUIs[ui].gameObject.SetActive(false);
            }
        }

        private void AddEventSystem()
        {
            Object eventSystem = Object.FindObjectOfType(typeof(EventSystem));
            if (ReferenceEquals(eventSystem, null))
            {
                eventSystem      = _resource.Instantiate("UI/EventSystem");
                eventSystem.name = "@EventSystem";
            }

            Object.DontDestroyOnLoad(eventSystem);
        }
	    
        private void AddGlobalUIRoot()
        {
            GlobalRoot = new GameObject {name = "@Global_UI_Root"};
            Object.DontDestroyOnLoad(GlobalRoot);

            foreach (var obj in _globalUIs.Values)
                obj.transform.SetParent(GlobalRoot.transform);
        }

        [Inject]
        public void InjectGlobalUI(UIRootLoadingScene uiRootLoadingScene)
        {
            AddGlobalUI(uiRootLoadingScene, eGlobalUI.LOADING);
        }

        private void AddGlobalUI(GlobalUIRootBase root, eGlobalUI type)
        {
            var tr = root.transform;
            if (!ReferenceEquals(GlobalRoot, null))
                tr.SetParent(GlobalRoot.transform);
            tr.localPosition = Vector3.zero;
            tr.localScale = Vector3.one;

            var obj = root.gameObject;
            obj.SetActive(false);
            
            _globalUIs.Add(type, root);
        }
    }
}
