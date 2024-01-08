using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    private GameObject              _prefab;
    private IObjectPool<GameObject> _pool;

    private Transform _root;
    private Transform Root
    {
        get
        {
            if (_root != null) return _root;
            
            var go = new GameObject() { name = $"@{_prefab.name}Pool" };
            _root = go.transform;

            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool   = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
    {
        if(go.activeSelf)
            _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

#region Funcs
    GameObject OnCreate()
    {
        GameObject go = Object.Instantiate(_prefab, Root, true);
        go.name = _prefab.name;
        return go;
    }

    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    void OnDestroy(GameObject go)
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
        private Dictionary<string, Pool> _pools = new();

        public GameObject Pop(GameObject prefab)
        {
            if (!_pools.ContainsKey(prefab.name))
                CreatePool(prefab);

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

        void CreatePool(GameObject original)
        {
            var pool = new Pool(original);
            _pools.Add(original.name, pool);
        }
    }
}
