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
        
        public void AddEvent(Action? clickOkEvent, Action? clickCancelEvent)
        {
            if (clickOkEvent != null)
                ClickOkEvent += clickOkEvent;
            
            if (clickCancelEvent != null)
                ClickCancelEvent += clickCancelEvent;
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