#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace GanShin
{
    public static class HumanoidUtils
    {
        private static readonly Dictionary<HumanBodyBones, HumanBodyBones> HumanBodyBonesParentMap = new()
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

            { HumanBodyBones.RightUpperLeg, HumanBodyBones.Hips },
            { HumanBodyBones.RightLowerLeg, HumanBodyBones.RightUpperLeg },
            { HumanBodyBones.RightFoot, HumanBodyBones.RightLowerLeg },

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

        public static readonly Dictionary<HumanBodyBones, List<HumanBodyBones>?> HumanBodyBonesChildrenMap = new()
        {
            { HumanBodyBones.Hips, new List<HumanBodyBones> { HumanBodyBones.Spine, HumanBodyBones.LeftUpperLeg, HumanBodyBones.RightUpperLeg } },
            { HumanBodyBones.Spine, new List<HumanBodyBones> { HumanBodyBones.Chest } },
            { HumanBodyBones.Chest, new List<HumanBodyBones> { HumanBodyBones.Neck, HumanBodyBones.LeftShoulder, HumanBodyBones.RightShoulder } },
            { HumanBodyBones.Neck, new List<HumanBodyBones> { HumanBodyBones.Head } },
            { HumanBodyBones.Head, new List<HumanBodyBones>() },

            { HumanBodyBones.LeftUpperLeg, new List<HumanBodyBones> { HumanBodyBones.LeftLowerLeg } },
            { HumanBodyBones.LeftLowerLeg, new List<HumanBodyBones> { HumanBodyBones.LeftFoot } },

            { HumanBodyBones.RightUpperLeg, new List<HumanBodyBones> { HumanBodyBones.RightLowerLeg } },
            { HumanBodyBones.RightLowerLeg, new List<HumanBodyBones> { HumanBodyBones.RightFoot } },

            { HumanBodyBones.LeftShoulder, new List<HumanBodyBones> { HumanBodyBones.LeftUpperArm } },
            { HumanBodyBones.LeftUpperArm, new List<HumanBodyBones> { HumanBodyBones.LeftLowerArm } },
            { HumanBodyBones.LeftLowerArm, new List<HumanBodyBones> { HumanBodyBones.LeftHand } },

            { HumanBodyBones.RightShoulder, new List<HumanBodyBones> { HumanBodyBones.RightUpperArm } },
            { HumanBodyBones.RightUpperArm, new List<HumanBodyBones> { HumanBodyBones.RightLowerArm } },
            { HumanBodyBones.RightLowerArm, new List<HumanBodyBones> { HumanBodyBones.RightHand } },
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
            HumanBodyBones.RightUpperLeg,
            HumanBodyBones.RightLowerLeg,
            HumanBodyBones.RightFoot,
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
            return HumanBodyBonesParentMap.TryGetValue(bone, out var parentBone) ?
                parentBone :
                HumanBodyBones.LastBone;
        }

        public static List<HumanBodyBones>? GetChildrenBones(HumanBodyBones bone)
        {
            return HumanBodyBonesChildrenMap.TryGetValue(bone, out var childrenBones) ?
                childrenBones :
                null;
        }
    }
}