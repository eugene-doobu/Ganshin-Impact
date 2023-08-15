using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GanShin.UI
{
    public enum EGlobalUI
    {
        LOADING_SCENE,
        CHARACTER_CUT_SCENE,
        DIMMED,
        POPUP_OK,
        POPUP_OK_CANCEL,
        TOAST,
        LOADING,
    }

    public partial class UIManager
    {
#region Define

        public const string EventSystemPath = "Prefabs/UI/EventSystem";

        public struct GlobalUIName
        {
            private const          string Root              = "Prefabs/UI/Global/";
            public static readonly string LoadingScene      = $"{Root}UI_LoadingScene";
            public static readonly string CharacterCutScene = $"{Root}UI_CharacterCutScene";
            public static readonly string Dimmed            = $"{Root}UI_Dimmed";
        }

#endregion Define

        private readonly Dictionary<EGlobalUI, GlobalUIRootBase> _globalUIs = new();

        public GlobalUIRootBase GetGlobalUI(EGlobalUI ui) =>
            _globalUIs.ContainsKey(ui) ? _globalUIs[ui] : null;

        private void OnGlobalUI(EGlobalUI ui, bool isOn)
        {
            if (isOn)
            {
                if (!_globalUIs.ContainsKey(ui)) return;
                _globalUIs[ui].gameObject.SetActive(true);
                _globalUIs[ui].InitializeContextData();
            }
            else
            {
                if (!_globalUIs.ContainsKey(ui)) return;
                _globalUIs[ui].ClearContextData();
                _globalUIs[ui].gameObject.SetActive(false);
            }
        }

        private void AddGlobalUIRoot()
        {
            GlobalRoot = new GameObject {name = "@Global_UI_Root"};
            Object.DontDestroyOnLoad(GlobalRoot);

            foreach (var obj in _globalUIs.Values)
                obj.transform.SetParent(GlobalRoot.transform);
        }

        [Inject]
        public void InjectEventSystem(EventSystem eventSystem)
        {
            eventSystem.name = "@EventSystem";
            Object.DontDestroyOnLoad(eventSystem);
        }

        [Inject]
        public void InjectGlobalUI(
            UIRootLoadingScene uiRootLoadingScene,
            UIRootCharacterCutScene uiRootCharacterCutScene,
            UIRootDimmed uiRootDimmed
            )
        {
            AddGlobalUI(uiRootLoadingScene, EGlobalUI.LOADING_SCENE);
            AddGlobalUI(uiRootCharacterCutScene, EGlobalUI.CHARACTER_CUT_SCENE);
            AddGlobalUI(uiRootDimmed, EGlobalUI.DIMMED);
        }

        private void AddGlobalUI(GlobalUIRootBase root, EGlobalUI type)
        {
            var tr = root.transform;
            if (!ReferenceEquals(GlobalRoot, null))
                tr.SetParent(GlobalRoot.transform);
            tr.localPosition = Vector3.zero;
            tr.localScale    = Vector3.one;

            var obj = root.gameObject;
            obj.SetActive(false);

            _globalUIs.Add(type, root);
        }

#region LOADING_SCENE
        public void SetLoadingSceneUiActive(bool isActive) => OnGlobalUI(EGlobalUI.LOADING_SCENE, isActive);
#endregion LOADING_SCENE

#region DIMMED
        public void SetDimmedUiActive(bool isActive) => OnGlobalUI(EGlobalUI.DIMMED, isActive);
#endregion DIMMED
    }
}