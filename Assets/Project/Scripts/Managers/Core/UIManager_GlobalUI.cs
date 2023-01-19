using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GanShin.UI
{
    public enum eGlobalUI
    {
        LOADING,
    }
    
    public partial class UIManager
    {
        private readonly Dictionary<eGlobalUI, GameObject> _globalUIs = new ();
        public GameObject GetGlobalUI(eGlobalUI ui) => 
            _globalUIs.ContainsKey(ui) ? _globalUIs[ui] : null;

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
        }

        private void InitializeGlobalUIs()
        {
            AddLoadingSceneUI();
        }

        private void AddLoadingSceneUI()
        {
            GameObject loadingUI = _resource.Instantiate("UI/Root/Canvas_LoadingScene");
            loadingUI.name = "@LoadingSceneUI";
            loadingUI.transform.SetParent(GlobalRoot.transform);
            loadingUI.transform.localPosition = Vector3.zero;
            loadingUI.transform.localScale = Vector3.one;
            loadingUI.SetActive(false);
            _globalUIs.Add(eGlobalUI.LOADING, loadingUI);
        }
    }
}
