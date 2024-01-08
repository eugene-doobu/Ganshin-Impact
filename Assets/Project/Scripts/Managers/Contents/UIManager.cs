using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.UI
{
    [UsedImplicitly]
    public partial class UIManager : ManagerBase
    {
        private readonly Dictionary<Type, Context> _dataContexts = new();
        private readonly List<Type> _willRemoveContexts = new();

        public GameObject GlobalRoot { get; private set; }

        [UsedImplicitly]
        private UIManager() {}
        
        public override void Initialize()
        {
            InjectEventSystem();
            InjectGlobalUI();
            AddGlobalUIRoot();
        }
        
        public void ClearContexts()
        {
            _willRemoveContexts.AddRange(_dataContexts.Keys);
            foreach (var key in _willRemoveContexts)
            {
                var context = _dataContexts[key];
                if (context is IDonDestroyContext)
                    continue;
                
                (context as IDisposable)?.Dispose();
                _dataContexts.Remove(key);
            }
            _willRemoveContexts.Clear();
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
    }
}