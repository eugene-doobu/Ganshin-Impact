#nullable enable

using System;
using System.Collections.Generic;
using GanShin.Resource;
using GanShin.UI.Space;
using UnityEngine;
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
        LOADING
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
        private readonly Dictionary<EGlobalUI, GlobalUIRootBase> _globalUIs = new();

        public GlobalUIRootBase? GetGlobalUI(EGlobalUI ui)
        {
            return _globalUIs.ContainsKey(ui) ? _globalUIs[ui] : null;
        }

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
            GlobalRoot = new GameObject { name = "@Global_UI_Root" };
            Object.DontDestroyOnLoad(GlobalRoot);

            foreach (var obj in _globalUIs.Values)
                obj.transform.SetParent(GlobalRoot.transform);
        }

        private void InjectEventSystem()
        {
            var eventSystem = Util.Instantiate("EventSystem.prefab");
            if (eventSystem == null)
                return;

            eventSystem.name = "@EventSystem";
            Object.DontDestroyOnLoad(eventSystem);
        }

        private void InjectGlobalUI()
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (resourceManager == null)
                return;

            var uiRootLoadingScene = resourceManager.Instantiate("UI_LoadingScene.prefab");
            if (uiRootLoadingScene != null)
                AddGlobalUI(uiRootLoadingScene.GetComponent<GlobalUIRootBase>(), EGlobalUI.LOADING_SCENE);

            var uiRootCharacterCutScene = resourceManager.Instantiate("UI_CharacterCutScene.prefab");
            if (uiRootCharacterCutScene != null)
                AddGlobalUI(uiRootCharacterCutScene.GetComponent<GlobalUIRootBase>(), EGlobalUI.CHARACTER_CUT_SCENE);

            var uiRootDimmed = resourceManager.Instantiate("UI_Dimmed.prefab");
            if (uiRootDimmed != null)
                AddGlobalUI(uiRootDimmed.GetComponent<GlobalUIRootBase>(), EGlobalUI.DIMMED);

            var uiRootPopup = resourceManager.Instantiate("UI_Popup.prefab");
            if (uiRootPopup != null)
                AddGlobalUI(uiRootPopup.GetComponent<GlobalUIRootBase>(), EGlobalUI.POPUP);

            var uiRootToast = resourceManager.Instantiate("UI_ToastPopup.prefab");
            if (uiRootToast != null)
                AddGlobalUI(uiRootToast.GetComponent<GlobalUIRootBase>(), EGlobalUI.TOAST);

            var uiRootLoading = resourceManager.Instantiate("UI_Loading.prefab");
            if (uiRootLoading != null)
                AddGlobalUI(uiRootLoading.GetComponent<GlobalUIRootBase>(), EGlobalUI.LOADING);
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

        public void SetLoadingSceneUiActive(bool isActive)
        {
            OnGlobalUI(EGlobalUI.LOADING_SCENE, isActive);
        }

#endregion LOADING_SCENE

#region DIMMED

        public void SetDimmedUiActive(bool isActive)
        {
            OnGlobalUI(EGlobalUI.DIMMED, isActive);
        }

#endregion DIMMED

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

#region Log

        public void AddLog(string log)
        {
            var logContext = GetContext<LogContext>();
            logContext.Items.Add(new LogItemContext(log));
        }

#endregion Log

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
    }
}