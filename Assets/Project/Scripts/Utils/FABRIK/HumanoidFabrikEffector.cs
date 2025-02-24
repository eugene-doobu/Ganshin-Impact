#nullable enable

using UnityEngine;

namespace GanShin.FABRIK
{
    public class HumanoidFabrikEffector
    {
#region Properties
	    public HumanBodyBones Bone { get; private set; }

	    public Transform Transform { get; }

	    public HumanoidFabrikEffector? Parent { get; }
#endregion Properties

        public HumanoidFabrikEffector(HumanBodyBones bone, Transform transform, HumanoidFabrikEffector? parent)
		{
			Bone = bone;

			Transform = transform;
			Parent    = parent;
		}
    }
}
