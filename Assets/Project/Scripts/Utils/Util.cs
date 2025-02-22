#nullable enable

using System.Collections.Generic;
using GanShin;
using GanShin.Resource;
using GanShin.UI;
using UnityEngine;

public static class Util
{
    private static readonly Dictionary<HumanBodyBones, HumanBodyBones> ParentMap = new()
    {
        // Hips → Spine → Chest → Neck → Head
        { HumanBodyBones.Spine, HumanBodyBones.Hips },
        { HumanBodyBones.Chest, HumanBodyBones.Spine },
        { HumanBodyBones.Neck, HumanBodyBones.Chest },
        { HumanBodyBones.Head, HumanBodyBones.Neck },

        // Hips → UpperLeg → LowerLeg → Foot → Toes
        { HumanBodyBones.LeftUpperLeg, HumanBodyBones.Hips },
        { HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftUpperLeg },
        { HumanBodyBones.LeftFoot, HumanBodyBones.LeftLowerLeg },
        { HumanBodyBones.LeftToes, HumanBodyBones.LeftFoot },

        { HumanBodyBones.RightUpperLeg, HumanBodyBones.Hips },
        { HumanBodyBones.RightLowerLeg, HumanBodyBones.RightUpperLeg },
        { HumanBodyBones.RightFoot, HumanBodyBones.RightLowerLeg },
        { HumanBodyBones.RightToes, HumanBodyBones.RightFoot },

        // Spine - Chest - Shoulders - Arm - Forearm - Hand
        // UpperChest는 인덱스 문제로 사용하지 않음
        // { HumanBodyBones.UpperChest, HumanBodyBones.Chest },

        { HumanBodyBones.LeftShoulder, HumanBodyBones.Chest /* UpperChest */ },
        { HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftShoulder },
        { HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftUpperArm },
        { HumanBodyBones.LeftHand, HumanBodyBones.LeftLowerArm },

        { HumanBodyBones.RightShoulder, HumanBodyBones.Chest /* UpperChest */ },
        { HumanBodyBones.RightUpperArm, HumanBodyBones.RightShoulder },
        { HumanBodyBones.RightLowerArm, HumanBodyBones.RightUpperArm },
        { HumanBodyBones.RightHand, HumanBodyBones.RightLowerArm },

        // 손가락 생략
    };

    public static readonly SortedSet<HumanBodyBones> IkEffectorBones = new()
    {
        HumanBodyBones.Hips,
        HumanBodyBones.Spine,
        HumanBodyBones.Chest,
        HumanBodyBones.Neck,
        HumanBodyBones.Head,
        HumanBodyBones.LeftUpperLeg,
        HumanBodyBones.LeftLowerLeg,
        HumanBodyBones.LeftFoot,
        HumanBodyBones.LeftToes,
        HumanBodyBones.RightUpperLeg,
        HumanBodyBones.RightLowerLeg,
        HumanBodyBones.RightFoot,
        HumanBodyBones.RightToes,
        HumanBodyBones.LeftShoulder,
        HumanBodyBones.LeftUpperArm,
        HumanBodyBones.LeftLowerArm,
        HumanBodyBones.LeftHand,
        HumanBodyBones.RightShoulder,
        HumanBodyBones.RightUpperArm,
        HumanBodyBones.RightLowerArm,
        HumanBodyBones.RightHand,
    };

    public static HumanBodyBones GetParentBone(HumanBodyBones bone)
    {
        return ParentMap.TryGetValue(bone, out var parentBone) ?
            parentBone :
            HumanBodyBones.LastBone;
    }

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