using System.Collections.Generic;
using GanShin.AssetManagement;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace GanShin.UI
{
	[UsedImplicitly]
    public class UIManager : IInitializable
    {
	    [Inject] private ResourceManager _resource;
		
	    int _order = 10;

	    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
	    UI_Scene _sceneUI = null;

	    public GameObject Root
	    {
		    get
		    {
			    GameObject root = GameObject.Find("@UI_Root");
			    if (root == null)
				    root = new GameObject { name = "@UI_Root" };
			    return root;
		    }
	    }

	    public UIManager()
	    {
		    SceneManager.sceneUnloaded += OnSceneUnLoaded;
	    }

	    public void Initialize()
	    {
		    Object eventSystem = Object.FindObjectOfType(typeof(EventSystem));
		    if (ReferenceEquals(eventSystem, null))
		    {
			    eventSystem      = _resource.Instantiate("UI/EventSystem");
			    eventSystem.name = "@EventSystem";
		    }
		    Object.DontDestroyOnLoad(eventSystem);
	    }

	    public void SetCanvas(GameObject go, bool sort = true)
	    {
	        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
	        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
	        canvas.overrideSorting = true;

	        if (sort)
	        {
	            canvas.sortingOrder = _order;
	            _order++;
	        }
	        else
	        {
	            canvas.sortingOrder = 0;
	        }
	    }

		public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
		{
			if (string.IsNullOrEmpty(name))
				name = typeof(T).Name;

			GameObject go = _resource.Instantiate($"UI/WorldSpace/{name}");
			if (parent != null)
				go.transform.SetParent(parent);

	        Canvas canvas = go.GetOrAddComponent<Canvas>();
	        canvas.renderMode = RenderMode.WorldSpace;
	        canvas.worldCamera = Camera.main;

			return Util.GetOrAddComponent<T>(go);
		}

		public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
		{
			if (string.IsNullOrEmpty(name))
				name = typeof(T).Name;

			GameObject go = _resource.Instantiate($"UI/SubItem/{name}");
			if (parent != null)
				go.transform.SetParent(parent);

			return Util.GetOrAddComponent<T>(go);
		}

		public T ShowSceneUI<T>(string name = null) where T : UI_Scene
		{
			if (string.IsNullOrEmpty(name))
				name = typeof(T).Name;

			GameObject go      = _resource.Instantiate($"UI/Scene/{name}");
			T          sceneUI = Util.GetOrAddComponent<T>(go);
	        _sceneUI = sceneUI;

			go.transform.SetParent(Root.transform);

			return sceneUI;
		}

		public T ShowPopupUI<T>(string name = null) where T : UI_Popup
	    {
	        if (string.IsNullOrEmpty(name))
	            name = typeof(T).Name;

	        GameObject go    = _resource.Instantiate($"UI/Popup/{name}");
	        T          popup = Util.GetOrAddComponent<T>(go);
	        _popupStack.Push(popup);

	        go.transform.SetParent(Root.transform);

			return popup;
	    }

	    public void ClosePopupUI(UI_Popup popup)
	    {
			if (_popupStack.Count == 0)
				return;

	        if (_popupStack.Peek() != popup)
	        {
	            GanDebugger.Log("Close Popup Failed!");
	            return;
	        }

	        ClosePopupUI();
	    }

	    public void ClosePopupUI()
	    {
	        if (_popupStack.Count == 0)
	            return;

	        UI_Popup popup = _popupStack.Pop();
	        _resource.Destroy(popup.gameObject);
	        popup = null;
	        _order--;
	    }

	    public void CloseAllPopupUI()
	    {
	        while (_popupStack.Count > 0)
	            ClosePopupUI();
	    }

	    private void OnSceneUnLoaded(Scene scene)
	    {
	        CloseAllPopupUI();
	        _sceneUI = null;
	    }
    }
}
