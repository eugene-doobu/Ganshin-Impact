#nullable enable

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class CollectionManagerContext<TKey, TContext> : GanContext, IDisposable where TContext : Context
    {
        private readonly Dictionary<TKey, TContext> _contextDict = new();

        [UsedImplicitly] public Collection<TContext> Items    { get; } = new();
        [UsedImplicitly] public int                  Count    => Items.Count;
        [UsedImplicitly] public bool                 HasItems => Items.Count > 0;
        [UsedImplicitly] public bool                 IsEmpty  => Items.Count == 0;

        public IReadOnlyDictionary<TKey, TContext> ContextDict => _contextDict;
        public event Action<TKey, TContext?>?      OnAdd;
        public event Action<TKey, TContext?>?      OnRemove;

        public bool TryGet(TKey key, out TContext? context)
        {
            return _contextDict.TryGetValue(key, out context);
        }

        public bool Contains(TKey key)
        {
            return _contextDict.ContainsKey(key);
        }

        protected void Add(TKey key, TContext context)
        {
            if (TryGet(key, out var oldContext))
            {
                OnRemove?.Invoke(key, oldContext);
                (oldContext as IDisposable)?.Dispose();

                if (oldContext != null)
                    Items.Remove(oldContext);
                _contextDict[key] = context;
            }
            else
            {
                _contextDict.Add(key, context);
            }

            Items.Add(context);
            OnAdd?.Invoke(key, context);
            InvokeCollectionChanged();
        }

        protected void Remove(TKey key)
        {
            if (!TryGet(key, out var context))
                return;

            OnRemove?.Invoke(key, context);
            (context as IDisposable)?.Dispose();

            if (context != null)
                Items.Remove(context);
            _contextDict.Remove(key);

            InvokeCollectionChanged();
        }

        public void Clear()
        {
            foreach (var item in _contextDict)
            {
                OnRemove?.Invoke(item.Key, item.Value);
                (item.Value as IDisposable)?.Dispose();
            }

            _contextDict.Clear();
            Items.Clear();

            InvokeCollectionChanged();
        }

        private void InvokeCollectionChanged()
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(HasItems));
            OnPropertyChanged(nameof(IsEmpty));
        }

#region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            OnDispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose()
        {
            if (_disposed) return;
            Clear();
            _disposed = true;
        }

#endregion IDisposable
    }
}