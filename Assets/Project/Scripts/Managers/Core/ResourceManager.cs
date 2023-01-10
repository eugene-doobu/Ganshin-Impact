using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using Zenject;

namespace GanShin.AssetManagement
{
    [UsedImplicitly]
    public class ResourceManager
    {
        [Inject] private PoolManager _pool;
        
        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);
    
                GameObject go = _pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }
    
            return Resources.Load<T>(path);
        }
    
        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"Prefabs/{path}");
            if (original == null)
            {
                GanDebugger.Log($"Failed to load prefab : {path}");
                return null;
            }
    
            if (original.GetComponent<Poolable>() != null)
                return _pool.Pop(original, parent).gameObject;
    
            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;
            return go;
        }
    
        public void Destroy(GameObject go)
        {
            if (go == null)
                return;
    
            Poolable poolable = go.GetComponent<Poolable>();
            if (poolable != null)
            {
                _pool.Push(poolable);
                return;
            }
    
            Object.Destroy(go);
        }
    }
}
