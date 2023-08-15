#nullable enable

using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace GanShin.UI
{
    public class UIRootPopup : GlobalUIRootBase
    {
        public PopupDataContext? PopupDataContext =>
            DataContext as PopupDataContext;

        protected override INotifyPropertyChanged InitializeDataContext() =>
            new PopupDataContext();
        
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

            if (isOkCancel) return;
            
            // Disable 이벤트는 가장 나중에 추가해야 한다.
            ClickOkEvent -= Disable;
            ClickOkEvent += Disable;
        }

        public override void InitializeContextData()
        {
            ClickCancelEvent += Disable;
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