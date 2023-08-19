#nullable enable

using System;
using System.ComponentModel;
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
                context.ClickOkEvent += clickOkEvent;
            
            if (clickCancelEvent != null)
                context.ClickCancelEvent += clickCancelEvent;

            cancelButtonRoot.SetActive(isOkCancel);

            foreach (var layoutRoot in layoutRoots)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
            
            // Disable 이벤트는 가장 나중에 추가해야 한다.
            context.ClickOkEvent     -= Disable;
            context.ClickOkEvent     += Disable;
            context.ClickCancelEvent -= Disable;
            context.ClickCancelEvent += Disable;
        }

        public override void InitializeContextData()
        {
        }

        public override void ClearContextData()
        {
            var context = PopupDataContext;
            if (context == null)
                return;

            context.ContentText = string.Empty;
            context.TitleText   = string.Empty;
            context.ClearEvent();
        }
    }
}