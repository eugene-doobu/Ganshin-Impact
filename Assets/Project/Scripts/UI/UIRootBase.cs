using System;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    [RequireComponent(typeof(ContextHolder))]
    public abstract class UIRootBase : MonoBehaviour
    {
        [UsedImplicitly] [Inject] protected UIManager UIManager { get; private set; }

        protected ContextHolder ContextHolder { get; private set; }

        public Context DataContext { get; protected set; }

        protected virtual void Awake()
        {
            ContextHolder         = GetComponent<ContextHolder>();
            DataContext           = InitializeDataContext();
            ContextHolder.Context = DataContext;
        }

        protected abstract Context InitializeDataContext();

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
    }
}