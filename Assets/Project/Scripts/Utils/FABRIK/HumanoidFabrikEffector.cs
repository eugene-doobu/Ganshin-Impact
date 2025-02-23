#nullable enable

using UnityEngine;

namespace GanShin.FABRIK
{
    public class HumanoidFabrikEffector
    {
#region Fields
	    private readonly Transform _transform;

	    private readonly HumanoidFabrikEffector? _parent;
#endregion Fields

#region Properties
	    public HumanBodyBones Bone { get; private set; }

	    public Transform Transform => _transform;

	    public Vector3    Position => Transform.position;
	    public Quaternion Rotation => Transform.rotation;
#endregion Properties

        public HumanoidFabrikEffector(HumanBodyBones bone, Transform transform, HumanoidFabrikEffector? parent)
		{
			Bone = bone;

			_transform = transform;
			_parent    = parent;
		}
    }
}
