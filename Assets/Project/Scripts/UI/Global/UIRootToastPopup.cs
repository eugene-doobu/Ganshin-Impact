#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GanShin.UI
{
    public class UIRootToastPopup : GlobalUIRootBase
    {
        [Header("UI Object")] [SerializeField] private Image background = null!;

        [SerializeField] private Animator animator = null!;

        [SerializeField] private RectTransform[] layoutRoots = null!;

        [Header("Options")] [SerializeField] private Color defaultColor;

        [SerializeField] private Color notificationColor;
        [SerializeField] private Color warningColor;
        [SerializeField] private Color errorColor;

        [SerializeField] private float defaultDuration         = 2.5f;
        [SerializeField] private float outDuration             = 0.2f;
        private readonly         int   _animationParamHashIn   = Animator.StringToHash("In");
        private readonly         int   _animationParamHashOut  = Animator.StringToHash("Out");
        private readonly         int   _animationParamHashWait = Animator.StringToHash("Wait");

        private CancellationTokenSource? _cancellationTokenSource;

        public ToastPopupDataContext? ToastPopupDataContext =>
            DataContext as ToastPopupDataContext;

        protected override Context InitializeDataContext()
        {
            return new ToastPopupDataContext();
        }

        public void SetContext(string title, string content, EToastType toastType)
        {
            var context = ToastPopupDataContext;
            if (context == null)
            {
                GanDebugger.LogError(GetType().Name, "Context is null");
                Disable();
                return;
            }

            context.ToastTitle   = title;
            context.ToastContent = content;

            foreach (var layoutRoot in layoutRoots)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);

            switch (toastType)
            {
                case EToastType.DEFAULT:
                    background.color = defaultColor;
                    break;
                case EToastType.NOTIFICATION:
                    background.color = notificationColor;
                    break;
                case EToastType.WARNING:
                    background.color = warningColor;
                    break;
                case EToastType.ERROR:
                    background.color = errorColor;
                    break;
            }

            ShowToast().Forget();
        }

        private async UniTaskVoid ShowToast()
        {
            if (animator == null)
            {
                GanDebugger.LogError(GetType().Name, "Animator is null");
                Disable();
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.NextFrame(_cancellationTokenSource.Token);

            animator.Play(_animationParamHashIn);
            await UniTask.Delay(TimeSpan.FromSeconds(defaultDuration),
                                cancellationToken: _cancellationTokenSource.Token);

            animator.Play(_animationParamHashOut);
            await UniTask.Delay(TimeSpan.FromSeconds(outDuration), cancellationToken: _cancellationTokenSource.Token);
        }

        public override void InitializeContextData()
        {
            CancelToken();
        }

        public override void ClearContextData()
        {
            var context = ToastPopupDataContext;
            if (context != null)
            {
                context.ToastTitle   = string.Empty;
                context.ToastContent = string.Empty;
            }

            CancelToken();
        }

        private void CancelToken()
        {
            if (animator != null)
                animator.Play(_animationParamHashWait);

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}