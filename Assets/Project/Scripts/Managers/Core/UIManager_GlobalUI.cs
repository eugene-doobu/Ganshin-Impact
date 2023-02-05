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
            
        public void OnGlobalUI(eGlobalUI ui, bool isOn)
        {
            if (isOn)
            {
                if (_globalUIs.ContainsKey(ui))
                    _globalUIs[ui].SetActive(true);
            }
            else
            {
                if (_globalUIs.ContainsKey(ui))
                    _globalUIs[ui].SetActive(false);
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
        }

        private void InitializeGlobalUIs()
        {
            AddLoadingSceneUI();
        }

        /// <summary>
        /// TODO: Zenject를 통한 방식으로 변경 필요
        /// </summary>
        private void AddLoadingSceneUI()
        {
            return;
            GameObject loadingUI = _resource.Instantiate("UI/Global/UI_LoadingScene", GlobalRoot.transform);
            loadingUI.transform.SetParent(GlobalRoot.transform);
            loadingUI.transform.localPosition = Vector3.zero;
            loadingUI.transform.localScale = Vector3.one;
            loadingUI.SetActive(false);
            _globalUIs.Add(eGlobalUI.LOADING, loadingUI);
        }
    }
}
