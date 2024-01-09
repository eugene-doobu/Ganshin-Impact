using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

internal class Pool
{
    private readonly IObjectPool<GameObject> _pool;
    private readonly GameObject              _prefab;

    private Transform _root;

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool   = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

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
        return go;
    }

    private void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    private void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    private void OnDestroy(GameObject go)
    {
        Object.Destroy(go);
    }

#endregion
}

namespace GanShin.Resource
{
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
            if (!_pools.ContainsKey(prefab.name))
                CreatePool(prefab, isDontDestroy);

            return _pools[prefab.name].Pop();
        }

        public bool Push(GameObject go)
        {
            if (!_pools.ContainsKey(go.name))
                return false;

            _pools[go.name].Push(go);
            return true;
        }

        public void Clear()
        {
            _pools.Clear();
        }

        private void CreatePool(GameObject original, bool isDontDestroy = false)
        {
            var pool = new Pool(original);
            _pools.Add(original.name, pool);
            if (isDontDestroy)
                Object.DontDestroyOnLoad(pool.Root);
        }
    }
}