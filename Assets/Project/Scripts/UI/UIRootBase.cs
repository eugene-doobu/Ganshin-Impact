#nullable enable

using System;
using DG.Tweening;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace GanShin.UI
{
    [RequireComponent(typeof(ContextHolder), typeof(CanvasGroup))]
    public abstract class UIRootBase : MonoBehaviour
    {
        [Header("UI Root Base")] [SerializeField]
        private float fadeDuration = 0.2f;

        private CanvasGroup _canvasGroup = null!;

        [UsedImplicitly]
        protected UIManager UIManager { get; private set; } = ProjectManager.Instance.GetManager<UIManager>();

        protected ContextHolder? ContextHolder { get; private set; }

        public Context? DataContext { get; protected set; }

        protected CanvasRoot? CanvasRoot { get; private set; }

        protected virtual void Awake()
        {
            _canvasGroup          = GetComponent<CanvasGroup>();
            ContextHolder         = GetComponent<ContextHolder>();
            DataContext           = InitializeDataContext();
            ContextHolder.Context = DataContext;
        }

        protected abstract Context? InitializeDataContext();

        public void CreateContext()
        {
            if (ContextHolder == null)
            {
                GanDebugger.LogError(nameof(UIRootBase), "ContextHolder is null");
                return;
            }

            ContextHolder.CreateContext = true;

            var newContext = Activator.CreateInstance(ContextHolder.ContextType);
            ContextHolder.SetContext(newContext, null);
        }

        public void InjectCanvasRoot(CanvasRoot root)
        {
            CanvasRoot = root;
        }

        public void Show()
        {
            _canvasGroup.DOFade(1, fadeDuration);
        }

        public void Hide()
        {
            _canvasGroup.DOFade(0, fadeDuration);
        }
    }
}