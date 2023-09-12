using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    [UsedImplicitly]
    public partial class UIManager : IInitializable
    {
        private readonly Dictionary<Type, Context> _dataContexts = new();

        public GameObject GlobalRoot { get; private set; } = null;

        [UsedImplicitly]
        public UIManager()
        {
        }

#region Context Management Interface

        public T GetContext<T>() where T : Context
        {
            if (_dataContexts.ContainsKey(typeof(T)))
                return _dataContexts[typeof(T)] as T;
            return null;
        }

        public T GetOrAddContext<T>() where T : Context, new()
        {
            if (_dataContexts.ContainsKey(typeof(T)))
                return _dataContexts[typeof(T)] as T;
            var context = new T();
            AddContext(context);
            return context;
        }

        public void AddContext<T>(T context) where T : Context
        {
            var result = _dataContexts.TryAdd(typeof(T), context);
            if (!result)
                GanDebugger.LogWarning(nameof(UIManager), "Context already exists");
        }

        public void RemoveContext<T>(T context) where T : Context
        {
            _dataContexts.Remove(typeof(T));
        }

#endregion Context Management Interface

        public void Initialize()
        {
            AddGlobalUIRoot();
        }
    }
}