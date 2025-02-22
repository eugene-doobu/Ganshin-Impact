#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace GanShin.FABRIK
{
    public class FabrikChain
    {
#region Fields
	    private readonly FabrikChain? _parent;

	    private readonly List<FabrikChain>            _children = new();
	    private readonly List<HumanoidFabrikEffector> _effectors;

	    private readonly float _sqrThreshold = 0.01F;

	    private float _summedWeight;

	    private Vector3 _target;
	    private bool _hasTarget;
#endregion Fields

#region Property
	    public bool IsEndChain => EndEffector.Bone is
		    HumanBodyBones.Head or
		    HumanBodyBones.LeftToes or
		    HumanBodyBones.RightToes or
		    HumanBodyBones.LeftHand or
		    HumanBodyBones.RightHand;

	    public int Layer { get; }

	    public Vector3 Target
	    {
		    get => _target;
		    set
		    {
			    _target = value;
			    _hasTarget = true;
		    }
	    }

	    private HumanoidFabrikEffector BaseEffector => _effectors[0];
	    public  HumanoidFabrikEffector EndEffector  => _effectors[^1];
#endregion Property

		public FabrikChain(FabrikChain? parent, List<HumanoidFabrikEffector> effectors, int layer)
		{
			for (var i = 1; i < effectors.Count; i++)
				effectors[i - 1].Length = Vector3.Distance(effectors[i].Transform.position, effectors[i - 1].Transform.position);

			_effectors = new List<HumanoidFabrikEffector>(effectors);
			Layer = layer;

			if (parent == null) return;
			_parent = parent;

			parent._children.Add(this);
		}

		public void CalculateSummedWeight()
		{
			_summedWeight = 0.0F;
			foreach (var child in _children)
				_summedWeight += child.EndEffector.Weight;
		}

		public void Backward()
		{
			if (!_hasTarget)
				return;

			var origin = BaseEffector.Position;
			if (_children.Count > 1)
				Target /= _summedWeight;

			if ((EndEffector.Position - Target).sqrMagnitude > _sqrThreshold)
			{
				EndEffector.Position = Target;
				for (var i = _effectors.Count - 2; i >= 0; i--)
				{
					var direction = Vector3.Normalize(_effectors[i].Position - _effectors[i + 1].Position);
					_effectors[i].Position = _effectors[i + 1].Position + direction * _effectors[i].Length;
				}
			}

			if (_parent != null)
				_parent.Target += BaseEffector.Position * EndEffector.Weight;

			BaseEffector.Position = origin;
		}

		private void Forward()
		{
			if (_effectors.Count == 1) return;
			_effectors[1].Position = BaseEffector.Position + BaseEffector.Rotation * Vector3.forward * BaseEffector.Length;

			for (var i = 2; i < _effectors.Count; i++)
			{
				var direction = Vector3.Normalize(_effectors[i].Position - _effectors[i - 1].Position);
				_effectors[i - 1].ApplyConstraints(direction);
				_effectors[i].Position = _effectors[i - 1].Position + _effectors[i - 1].Rotation * Vector3.forward * _effectors[i - 1].Length;
			}

			if (_children.Count == 0) return;

			Target = Vector3.zero;
			_hasTarget = false;

			var childrenDir = Vector3.zero;
			foreach(var child in _children)
				childrenDir += Vector3.Normalize(child._effectors[1].Position - EndEffector.Position);
			childrenDir /= _children.Count;

			EndEffector.ApplyConstraints(childrenDir);
		}

		public void ForwardMulti()
		{
			Forward();

			for (var i = 1; i < _effectors.Count; i++)
				_effectors[i].UpdateTransform();

			foreach (var child in _children)
				child.ForwardMulti();
		}
    }
}
