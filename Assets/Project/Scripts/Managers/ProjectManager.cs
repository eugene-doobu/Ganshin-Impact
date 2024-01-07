#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GanShin
{
    public class ProjectManager
    {
        #region Fields
        private static ProjectManager? _instance;
    
        private readonly Dictionary<Type, ManagerBase> _managers = new();
        
        private Action? _onInitialized;
        
        private CancellationTokenSource? _cts;
        #endregion Fields

        #region Properties
        public static ProjectManager Instance => _instance ??= new ProjectManager();
        
        public event Action OnInitialized
        {
            add => _onInitialized += value;
            remove => _onInitialized -= value;
        }
        #endregion Properties
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.InitializeManagers();
        }
    
        public T? GetManager<T>() where T : ManagerBase
        {
            var type = typeof(T);
            if (!_managers.ContainsKey(type))
                return null;

            return _managers[type] as T;
        }
    
        private void InitializeManagers()
        {
            RefreshCts();
            
            _onInitialized = null;
            _managers.Clear();
        
            var assemblies = Assembly.GetExecutingAssembly();
            var types      = assemblies.GetTypes();
        
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (!type.IsSubclassOf(typeof(ManagerBase))) 
                    continue;
            
                var manager = Activator.CreateInstance(type) as ManagerBase;
            
                if (manager == null)
                    continue;
            
                _managers.Add(type, manager);
                manager.Initialize();
            }
            
            _onInitialized?.Invoke();
            
            Tick().Forget();
            LateTick().Forget();
        }

        private void CancelCts()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private void RefreshCts()
        {
            CancelCts();
            _cts = new CancellationTokenSource();
        }

        private async UniTask Tick()
        {
            while (_cts != null && !_cts.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                foreach (var manager in _managers.Values)
                    manager.Tick();
            }
        }
        
        private async UniTask LateTick()
        {
            while (_cts != null && !_cts.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, _cts.Token);
                foreach (var manager in _managers.Values)
                    manager.LateTick();
            }
        }
    }
}
