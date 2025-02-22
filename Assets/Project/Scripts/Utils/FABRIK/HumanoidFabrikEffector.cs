#nullable enable

using UnityEngine;

namespace GanShin.FABRIK
{
    public class HumanoidFabrikEffector
    {
#region Fields
	    private readonly float     _weight;
	    private readonly Transform _transform;

	    private readonly HumanoidFabrikEffector? _parent;

	    private Vector3    _position;
	    private Quaternion _rotation;
#endregion Fields

#region Priperties
	    public HumanBodyBones Bone { get; private set; }

	    public Quaternion Rotation
	    {
		    get => _parent != null ? _rotation : _transform.rotation;
		    set => _rotation = value;
	    }

	    public Vector3 UpAxisConstraint      { get; set; } = Vector3.up;
	    public Vector3 ForwardAxisConstraint { get; set; } = Vector3.forward;
	    public float   SwingConstraint       { get; set; } = float.NaN;
	    public float   TwistConstraint       { get; set; } = float.NaN;
	    public float   AngularConstraint     { get; set; } = float.NaN;

	    public float Length { get; set; }

	    public float SwingConstraintRad => SwingConstraint * 0.5F * Mathf.Deg2Rad;
	    public float TwistConstraintRad => TwistConstraint * 0.5F * Mathf.Deg2Rad;

	    public bool SwingConstrained   => !float.IsNaN(SwingConstraint);
	    public bool TwistConstrained   => !float.IsNaN(TwistConstraint);
	    public bool AngularConstrained => !float.IsNaN(AngularConstraint);

	    public float     Weight    => _weight;
	    public Transform Transform => _transform;

	    public Vector3 Position
	    {
		    get => _parent != null ? _position : _transform.position;
		    set => _position = value;
	    }
#endregion Priperties

        public HumanoidFabrikEffector(HumanBodyBones bone, Transform transform, HumanoidFabrikEffector? parent, float weight = 1.0f)
		{
			Bone = bone;

			_transform = transform;
			_parent    = parent;
			_weight    = weight;

			Position = _transform.position;
			Rotation = _transform.rotation;
		}

		public void ApplyConstraints(Vector3 direction)
		{
			if (_parent != null)
			{
				var position = _transform.position;
				if (AngularConstrained)
				{
					var phi = Vector3.SignedAngle(position, Position, UpAxisConstraint);
					var angVel = phi * Time.deltaTime;
					if (!float.IsNaN(angVel) && Mathf.Abs(angVel) > Mathf.Abs(AngularConstraint * Time.deltaTime))
						Position = Quaternion.AngleAxis(Mathf.Sign(phi)*AngularConstraint*Time.deltaTime, UpAxisConstraint) * _transform.position;
				}
				if (!SwingConstrained && !TwistConstrained)
				{
					Rotation = Quaternion.LookRotation(direction, _parent.Rotation * Vector3.up);
				}
				else
				{
					var rotationGlobal = Quaternion.LookRotation(_parent.Rotation * ForwardAxisConstraint, _parent.Rotation * UpAxisConstraint);
					var rotationLocal = Quaternion.Inverse(rotationGlobal) * Quaternion.LookRotation(direction);

					rotationLocal.Decompose(Vector3.forward, out var swing, out var twist);

					if (SwingConstrained)
						swing = swing.Constrain(SwingConstraint);

					if (TwistConstrained)
						twist = twist.Constrain(TwistConstraint);

					Rotation = rotationGlobal * swing * twist;
				}
			}
			else
			{
				Rotation = Quaternion.LookRotation(direction);
			}
		}

		public void UpdateTransform()
		{
			var x90 = new Quaternion(Mathf.Sqrt(0.5F), 0.0F, 0.0F, Mathf.Sqrt(0.5F));

			_transform.rotation = Rotation * x90;
			_transform.position = Position;
		}
    }
}
