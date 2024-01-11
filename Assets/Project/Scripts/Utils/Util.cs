#nullable enable

using GanShin;
using GanShin.Resource;
using GanShin.UI;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        var component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject? FindChild(GameObject go, string? name = null, bool recursive = false)
    {
        var transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T? LoadAsset<T>(string key) where T : Object
    {
        var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
        if (resourceManager == null)
            return null;

        return resourceManager.Load<T>(key);
    }

    public static GameObject? Instantiate(string key, Transform? parent = null, bool pooling = false)
    {
        var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
        return resourceManager?.Instantiate(key, parent, pooling);
    }
    
    public static T? GetContext<T>() where T : GanContext
    {
        var contextManager = ProjectManager.Instance.GetManager<UIManager>();
        return contextManager?.GetContext<T>();
    }

    public static T? FindChild<T>(GameObject go, string? name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false)
            for (var i = 0; i < go.transform.childCount; i++)
            {
                var transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    var component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        else
            foreach (var component in go.GetComponentsInChildren<T>())
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;

        return null;
    }
}