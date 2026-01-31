#nullable enable

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Event
{
    [UsedImplicitly]
    public class EventManager : ManagerBase
    {
        private readonly Dictionary<eEventType, Action?> _events = new();
        private readonly Dictionary<eEventType, Delegate?> _eventsWithParam = new();

        [UsedImplicitly]
        public EventManager()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _events.Clear();
            _eventsWithParam.Clear();
        }

        #region No Parameter Events

        public void Subscribe(eEventType eventType, Action callback)
        {
            if (callback == null)
            {
                Debug.LogWarning($"[EventManager] Subscribe: callback is null for event type {eventType}");
                return;
            }

            _events.TryAdd(eventType, null);

            _events[eventType] -= callback;
            _events[eventType] += callback;
        }

        public void Unsubscribe(eEventType eventType, Action callback)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType] -= callback;
        }

        public void Publish(eEventType eventType)
        {
            if (_events.TryGetValue(eventType, out var action))
                action?.Invoke();
        }

        #endregion No Parameter Events

        #region Generic Parameter Events

        public void Subscribe<T>(eEventType eventType, Action<T> callback)
        {
            if (callback == null)
            {
                Debug.LogWarning($"[EventManager] Subscribe<{typeof(T).Name}>: callback is null for event type {eventType}");
                return;
            }

            _eventsWithParam.TryAdd(eventType, null);

            _eventsWithParam[eventType] = Delegate.Remove(_eventsWithParam[eventType], callback);
            _eventsWithParam[eventType] = Delegate.Combine(_eventsWithParam[eventType], callback);
        }

        public void Unsubscribe<T>(eEventType eventType, Action<T> callback)
        {
            if (_eventsWithParam.ContainsKey(eventType))
                _eventsWithParam[eventType] = Delegate.Remove(_eventsWithParam[eventType], callback);
        }

        public void Publish<T>(eEventType eventType, T param)
        {
            if (_eventsWithParam.TryGetValue(eventType, out var del))
            {
                if (del is Action<T> action)
                {
                    action.Invoke(param);
                }
                else if (del != null)
                {
                    Debug.LogWarning($"[EventManager] Publish<{typeof(T).Name}>: Type mismatch for event type {eventType}. Expected Action<{typeof(T).Name}>, but got {del.GetType().Name}");
                }
            }
        }

        #endregion Generic Parameter Events

        public void Clear()
        {
            _events.Clear();
            _eventsWithParam.Clear();
        }
    }
}
