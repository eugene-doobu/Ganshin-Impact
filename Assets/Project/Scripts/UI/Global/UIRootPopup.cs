#nullable enable

using System;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GanShin.UI
{
    public class UIRootPopup : GlobalUIRootBase
    {
        public PopupDataContext? PopupDataContext =>
            DataContext as PopupDataContext;

        protected override INotifyPropertyChanged InitializeDataContext() =>
            new PopupDataContext();

        [SerializeField] private GameObject      cancelButtonRoot = null!;
        [SerializeField] private RectTransform[] layoutRoots      = null!;
        
        public event Action? ClickOkEvent;
        public event Action? ClickCancelEvent;

        [UsedImplicitly]
        public void ClickOk() => ClickOkEvent?.Invoke();
        [UsedImplicitly]
        public void ClickCancel() => ClickCancelEvent?.Invoke();

        public void SetContext(string title, string content, bool isOkCancel, Action? clickOkEvent, Action? clickCancelEvent = null)
        {
            var context = PopupDataContext;
            if (context == null)
            {
                GanDebugger.LogError(GetType().Name, "Context is null");
                Disable();
                return;
            }
            
            context.TitleText   = title;
            context.ContentText = content;
            context.IsOkCancel  = isOkCancel;

            if (clickOkEvent != null)
                ClickOkEvent += clickOkEvent;
            
            if (clickCancelEvent != null)
                ClickCancelEvent += clickCancelEvent;

            cancelButtonRoot.SetActive(isOkCancel);

            foreach (var layoutRoot in layoutRoots)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
            
            // Disable 이벤트는 가장 나중에 추가해야 한다.
            ClickOkEvent     -= Disable;
            ClickOkEvent     += Disable;
            ClickCancelEvent -= Disable;
            ClickCancelEvent += Disable;
        }

        public override void InitializeContextData()
        {
        }

        public override void ClearContextData()
        {
            var context = PopupDataContext;
            if (context != null)
            {
                context.ContentText = string.Empty;
                context.TitleText   = string.Empty;   
            }

            ClickOkEvent = null;
            ClickCancelEvent = null;
        }
    }
}