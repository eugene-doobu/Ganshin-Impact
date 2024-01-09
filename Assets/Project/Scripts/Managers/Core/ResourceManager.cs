#nullable enable

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GanShin.Resource
{
    [UsedImplicitly]
    public class ResourceManager : ManagerBase
    {
        /// <summary>
        ///     프로세스 종료시까지 유지되는 리소스
        /// </summary>
        private readonly Dictionary<string, Object> _dontDestroyOnLoadResources = new();

        /// <summary>
        ///     씬 전환시 제거되는 리소스
        /// </summary>
        private readonly Dictionary<string, Object> _resources = new();

        [UsedImplicitly]
        public ResourceManager()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _dontDestroyOnLoadResources.Clear();
            _resources.Clear();
        }

        public T? Load<T>(string key) where T : Object
        {
            if (_resources.TryGetValue(key, out Object resource))
                return resource as T;

            if (_dontDestroyOnLoadResources.TryGetValue(key, out resource))
                return resource as T;

            return null;
        }

        public GameObject? Instantiate(string key, Transform? parent = null, bool pooling = false)
        {
            var prefab = Load<GameObject>($"{key}");
            if (prefab == null)
            {
                GanDebugger.Log($"Failed to load prefab : {key}");
                return null;
            }

            var isDontDestroy = _dontDestroyOnLoadResources.ContainsKey(key);
            if (pooling)
            {
                var pool = ProjectManager.Instance.GetManager<PoolManager>();

                if (pool == null)
                {
                    GanDebugger.LogError("Failed to get pool manager");
                    return null;
                }

                return pool.Pop(prefab, isDontDestroy);
            }

            var go = Object.Instantiate(prefab, parent);
            go.name = prefab.name;

            if (isDontDestroy)
                Object.DontDestroyOnLoad(go);

            return go;
        }

        public void Destroy(GameObject go)
        {
            if (go == null)
                return;

            if (ProjectManager.Instance.GetManager<PoolManager>()?.Push(go) ?? false)
                return;

            Object.Destroy(go);
        }

#region Addressable

        public async UniTask LoadAsync<T>(string key, bool isDonDestroy = false) where T : Object
        {
            if (_resources.ContainsKey(key))
                return;

            if (_dontDestroyOnLoadResources.ContainsKey(key))
                return;

            var loadKey = key;
            if (key.Contains(".sprite"))
                loadKey = $"{key}[{key.Replace(".sprite", "")}]";

            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            await asyncOperation;

            if (isDonDestroy)
                _dontDestroyOnLoadResources.Add(key, asyncOperation.Result);
            else
                _resources.Add(key, asyncOperation.Result);
        }

        public async UniTask LoadAllAsync<T>(string label, bool isDonDestroy = false) where T : Object
        {
            var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
            await opHandle;

            GanDebugger.Log($"LoadAllAsync : {label}");

            foreach (var result in opHandle.Result)
                if (result.PrimaryKey.Contains(".sprite"))
                    await LoadAsync<Sprite>(result.PrimaryKey, isDonDestroy);
                else
                    await LoadAsync<T>(result.PrimaryKey, isDonDestroy);
        }

        public void Release(string key)
        {
            if (!_resources.TryGetValue(key, out var resource)) return;
            Addressables.Release(resource);
            _resources.Remove(key);
        }

        public void ReleaseAll()
        {
            foreach (var resource in _resources)
                Addressables.Release(resource.Value);

            _resources.Clear();
        }

#endregion Addressable
    }
}