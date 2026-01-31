#nullable enable

using System.Collections.Generic;
using GanShin.GanObject;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace GanShin.Resource
{
    internal class Pool
    {
        private readonly IObjectPool<GameObject>          _pool;
        private readonly GameObject                       _prefab;
        private readonly Dictionary<GameObject, Actor?> _actorCache = new();

        private Transform? _root;

        public Pool(GameObject prefab)
        {
            _prefab = prefab;
            _pool   = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
        }

        public Transform? RootOrNull => _root;

        public Transform Root
        {
            get
            {
                if (_root != null) return _root;

                var go = new GameObject { name = $"@{_prefab.name}Pool" };
                _root = go.transform;

                return _root;
            }
        }

        public void Push(GameObject go)
        {
            if (go.activeSelf)
                _pool.Release(go);
        }

        public GameObject Pop()
        {
            return _pool.Get();
        }

    #region Funcs

        private GameObject OnCreate()
        {
            var go = Object.Instantiate(_prefab, Root, true);
            go.name = _prefab.name;

            go.TryGetComponent(out Actor? actor);
            _actorCache[go] = actor;

            if (actor != null)
                actor.Pooling = true;

            return go;
        }

        private void OnGet(GameObject go)
        {
            if (_actorCache.TryGetValue(go, out var actor) && actor != null)
            {
                actor.Pooling = true;
                actor.OnSpawn();
            }

            go.SetActive(true);
        }

        private void OnRelease(GameObject go)
        {
            if (_actorCache.TryGetValue(go, out var actor) && actor != null)
                actor.OnDespawn();

            go.SetActive(false);
        }

        private void OnDestroy(GameObject go)
        {
            _actorCache.Remove(go);
            Object.Destroy(go);
        }
    #endregion
    }

    [UsedImplicitly]
    public class PoolManager : ManagerBase
    {
        private readonly Dictionary<string, Pool> _pools = new();

        [UsedImplicitly]
        public PoolManager()
        {
        }

        public GameObject Pop(GameObject prefab, bool isDontDestroy = false)
        {
            if (!_pools.TryGetValue(prefab.name, out var pool))
            {
                pool = CreatePool(prefab, isDontDestroy);
            }

            return pool.Pop();
        }

        public bool Push(GameObject go)
        {
            if (!_pools.TryGetValue(go.name, out var pool))
                return false;

            pool.Push(go);
            return true;
        }

        public void Clear()
        {
            foreach (var pool in _pools.Values)
            {
                if (pool.RootOrNull != null)
                    Object.Destroy(pool.RootOrNull.gameObject);
            }

            _pools.Clear();
        }

        private Pool CreatePool(GameObject original, bool isDontDestroy = false)
        {
            var pool = new Pool(original);
            _pools.Add(original.name, pool);
            if (isDontDestroy)
                Object.DontDestroyOnLoad(pool.Root);
            return pool;
        }
    }
}