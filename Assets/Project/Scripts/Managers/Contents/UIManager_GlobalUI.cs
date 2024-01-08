#nullable enable

using System;
using System.Collections.Generic;
using GanShin.Space.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace GanShin.UI
{
    public enum EGlobalUI
    {
        LOADING_SCENE,
        CHARACTER_CUT_SCENE,
        DIMMED,
        POPUP,
        TOAST,
        LOADING,
    }
    
    public enum EToastType
    {
        DEFAULT,
        NOTIFICATION,
        WARNING,
        ERROR
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
            public static readonly string Popup             = $"{Root}UI_Popup";
            public static readonly string Toast             = $"{Root}UI_ToastPopup";
            public static readonly string Loading           = $"{Root}UI_Loading";
        }
#endregion Define

        private readonly Dictionary<EGlobalUI, GlobalUIRootBase> _globalUIs = new();

        public GlobalUIRootBase? GetGlobalUI(EGlobalUI ui) =>
            _globalUIs.ContainsKey(ui) ? _globalUIs[ui] : null;

        private void OnGlobalUI(EGlobalUI ui, bool isOn)
        {
            if (isOn)
            {
                if (!_globalUIs.ContainsKey(ui)) return;
                _globalUIs[ui].Enable();
            }
            else
            {
                if (!_globalUIs.ContainsKey(ui)) return;
                _globalUIs[ui].Disable();
            }
        }

        private void AddGlobalUIRoot()
        {
            GlobalRoot = new GameObject {name = "@Global_UI_Root"};
            Object.DontDestroyOnLoad(GlobalRoot);

            foreach (var obj in _globalUIs.Values)
                obj.transform.SetParent(GlobalRoot.transform);
        }

        // TODO: Addressable
        private void InjectEventSystem()
        {
            var eventSystem = Object.Instantiate(Resources.Load<EventSystem>(EventSystemPath));
            
            eventSystem.name = "@EventSystem";
            Object.DontDestroyOnLoad(eventSystem);
        }

        // TODO: Addressable
        private void InjectGlobalUI()
        {
            var uiRootLoadingScene      = Object.Instantiate(Resources.Load<UIRootLoadingScene>(GlobalUIName.LoadingScene));
            var uiRootCharacterCutScene = Object.Instantiate(Resources.Load<UIRootCharacterCutScene>(GlobalUIName.CharacterCutScene));
            var uiRootDimmed            = Object.Instantiate(Resources.Load<UIRootDimmed>(GlobalUIName.Dimmed));
            var uiRootPopup             = Object.Instantiate(Resources.Load<UIRootPopup>(GlobalUIName.Popup));
            var uiRootToast             = Object.Instantiate(Resources.Load<UIRootToastPopup>(GlobalUIName.Toast));
            var uiRootLoading           = Object.Instantiate(Resources.Load<UIRootLoadingPopup>(GlobalUIName.Loading));
            
            AddGlobalUI(uiRootLoadingScene, EGlobalUI.LOADING_SCENE);
            AddGlobalUI(uiRootCharacterCutScene, EGlobalUI.CHARACTER_CUT_SCENE);
            AddGlobalUI(uiRootDimmed, EGlobalUI.DIMMED);
            AddGlobalUI(uiRootPopup, EGlobalUI.POPUP);
            AddGlobalUI(uiRootToast, EGlobalUI.TOAST);
            AddGlobalUI(uiRootLoading, EGlobalUI.LOADING);
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

#region Popup
        public void SetPopupOk(string title, string content, Action? okAction = null)
        {
            var popup = GetGlobalUI(EGlobalUI.POPUP) as UIRootPopup;
            if (popup == null)
                return;

            OnGlobalUI(EGlobalUI.POPUP, true);
            popup.SetContext(title, content, false, okAction);
        }

        public void SetPopupOkCancel(string title, string content, Action? okAction = null, Action? cancelAction = null)
        {
            var popup = GetGlobalUI(EGlobalUI.POPUP) as UIRootPopup;
            if (popup == null)
                return;

            OnGlobalUI(EGlobalUI.POPUP, true);
            popup.SetContext(title, content, true, okAction, cancelAction);
        }
#endregion Popup

#region Toast
        public void SetToast(string title, string content, EToastType type = EToastType.DEFAULT)
        {
            var toast = GetGlobalUI(EGlobalUI.TOAST) as UIRootToastPopup;
            if (toast == null)
                return;

            OnGlobalUI(EGlobalUI.TOAST, true);
            toast.SetContext(title, content, type);
        }
#endregion Toast

#region Loading
        public void ShowLoadingUI(int hash)
        {
            var loading = GetGlobalUI(EGlobalUI.LOADING) as UIRootLoadingPopup;
            if (loading == null)
                return;

            OnGlobalUI(EGlobalUI.LOADING, true);
            loading.AddHash(hash);
        }
        
        public void HideLoadingUI(int hash)
        {
            var loading = GetGlobalUI(EGlobalUI.LOADING) as UIRootLoadingPopup;
            if (loading == null)
                return;

            loading.RemoveHash(hash);
            if (loading.IsEmpty())
                OnGlobalUI(EGlobalUI.LOADING, false);
        }
#endregion Loading

#region Log
        public void AddLog(string log)
        {
            var logContext = GetContext<LogContext>();
            logContext.Items.Add(new LogItemContext(log));
        }
#endregion Log
    }
}