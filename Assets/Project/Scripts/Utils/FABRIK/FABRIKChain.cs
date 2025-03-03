#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace GanShin.FABRIK
{
    public class FabrikChain
    {
	    private const int   ITERATIONS         = 10;
	    private const float DELTA              = 0.001f;
	    private const float SNAP_BACK_STRENGTH = 1f;

#region Fields
	    private readonly List<HumanoidFabrikEffector> _effectors;

	    private bool _hasTarget;
#endregion Fields

#region Property
	    public bool IsEndChain => EndEffector.Bone is
		    HumanBodyBones.Head or
		    HumanBodyBones.LeftFoot or
		    HumanBodyBones.RightFoot or
		    HumanBodyBones.LeftHand or
		    HumanBodyBones.RightHand;

	    public Vector3 TargetTransform
	    {
		    set
		    {
			    if (_target != null)
				    _target.position = value;
			    _hasTarget = true;
		    }
	    }

	    public Quaternion TargetRotation
	    {
		    set
		    {
			    if (_target != null)
					_target.rotation = value;
			    _hasTarget = true;
		    }
	    }

	    public int Layer { get; private set; }

	    private HumanoidFabrikEffector BaseEffector => _effectors[0];
	    public  HumanoidFabrikEffector EndEffector  => _effectors[^1];
#endregion Property

	    private readonly HumanoidFabrikEffector[]? _bones;
	    private readonly HumanoidFabrikEffector?   _root;

	    private readonly float[]?      _bonesLength; //Target to Origin
	    private readonly float         _completeLength;
	    private readonly Vector3[]?    _positions;
	    private readonly Vector3[]?    _startDirectionSucc;
	    private readonly Quaternion[]? _startRotationBone;
	    private readonly Quaternion    _startRotationTarget;

	    private readonly Transform? _target;

	    private Transform? _pole;

		public FabrikChain(List<HumanoidFabrikEffector> effectors, int layer, Animator animator, IReadOnlyDictionary<HumanBodyBones, HumanoidFabrikEffector> dictionary)
		{
			_effectors = new List<HumanoidFabrikEffector>(effectors);

			Layer = layer;

			var chainLength = _effectors.Count - 2;
			if (chainLength < 1)
			{
				return;
			}

			//initial array
			_bones              = new HumanoidFabrikEffector[chainLength + 1];
			_positions          = new Vector3[chainLength + 1];
			_bonesLength        = new float[chainLength];
			_startDirectionSucc = new Vector3[chainLength + 1];
			_startRotationBone  = new Quaternion[chainLength + 1];

			_root = EndEffector;
			for (var i = 0; i <= chainLength; i++)
			{
				if (_root == null)
				{
					GanDebugger.LogError("Root is null");
					return;
				}
				_root = _root.Parent;
			}

			if (_target == null)
			{
				_target = new GameObject("Target").transform;
				SetPositionRootSpace(_target, GetPositionRootSpace(EndEffector.Transform));
			}
			_startRotationTarget = GetRotationRootSpace(_target);

			if (_pole == null || EndEffector.Bone != HumanBodyBones.Head)
				CreateNewPole(animator, dictionary);

			//init data
			var current = EndEffector;
			_completeLength = 0;
			for (var i = _bones.Length - 1; i >= 0; i--)
			{
				_bones[i]             = current;
				_startRotationBone[i] = GetRotationRootSpace(current.Transform);

				if (i == _bones.Length - 1)
				{
					_startDirectionSucc[i] = GetPositionRootSpace(_target) - GetPositionRootSpace(current.Transform);
				}
				else
				{
					_startDirectionSucc[i] =  GetPositionRootSpace(_bones[i + 1].Transform) - GetPositionRootSpace(current.Transform);
					_bonesLength[i]        =  _startDirectionSucc[i].magnitude;
					_completeLength        += _bonesLength[i];
				}

				var parent = HumanoidUtils.GetParentBone(current.Bone);
				if (parent == HumanBodyBones.LastBone)
					break;

				// GanDebugger.Log("Bone: " + current.Bone + " Parent: " + parent);
				current = dictionary[parent];
			}
		}

		public void SolveIK()
		{
			if (!_hasTarget)
				return;

			if (_target == null)
				return;

			_hasTarget = false;

			for (int i = 0; i < _bones.Length; i++)
                _positions[i] = GetPositionRootSpace(_bones[i].Transform);

            var targetPosition = GetPositionRootSpace(_target);
            var targetRotation = GetRotationRootSpace(_target);

            var val1 = (targetPosition - GetPositionRootSpace(_bones[0].Transform)).sqrMagnitude;
            var isReachable = val1 >= _completeLength * _completeLength;
            if (isReachable)
            {
	            // just stretch it
	            var direction = (targetPosition - _positions[0]).normalized;
	            for (int i = 1; i < _positions.Length; i++)
		            _positions[i] = _positions[i - 1] + direction * _bonesLength[i - 1];
            }
            else
            {
	            for (var i = 0; i < _positions.Length - 1; i++)
		            _positions[i + 1] = Vector3.Lerp(_positions[i + 1], _positions[i] + _startDirectionSucc[i], SNAP_BACK_STRENGTH);

                for (var iteration = 0; iteration < ITERATIONS; iteration++)
                {
                    // https://www.youtube.com/watch?v=UNoX65PRehA
                    // back
                    for (var i = _positions.Length - 1; i > 0; i--)
                    {
                        if (i == _positions.Length - 1)
                            _positions[i] = targetPosition; //set it to target
                        else
                            _positions[i] = _positions[i + 1] + (_positions[i] - _positions[i + 1]).normalized * _bonesLength[i];
                    }

                    // forward
                    for (var i = 1; i < _positions.Length; i++)
                        _positions[i] = _positions[i - 1] + (_positions[i] - _positions[i - 1]).normalized * _bonesLength[i - 1];

                    // close enough
                    if ((_positions[^1] - targetPosition).sqrMagnitude < DELTA * DELTA)
                        break;
                }
            }

            //move towards pole
            if (_pole != null)
            {
                var polePosition = GetPositionRootSpace(_pole);
                for (var i = 1; i < _positions.Length - 1; i++)
                {
                    var plane = new Plane(_positions[i + 1] - _positions[i - 1], _positions[i - 1]);
                    var projectedPole = plane.ClosestPointOnPlane(polePosition);
                    var projectedBone = plane.ClosestPointOnPlane(_positions[i]);
                    var angle = Vector3.SignedAngle(projectedBone - _positions[i - 1], projectedPole - _positions[i - 1], plane.normal);
                    _positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (_positions[i] - _positions[i - 1]) + _positions[i - 1];
                }
            }

            //set position & rotation
            for (var i = 0; i < _positions.Length; i++)
            {
	            if (i == _positions.Length - 1)
		            SetRotationRootSpace(_bones[i].Transform, Quaternion.Inverse(targetRotation) * _startRotationTarget * Quaternion.Inverse(_startRotationBone[i]));
	            else
		            SetRotationRootSpace(_bones[i].Transform, Quaternion.FromToRotation(_startDirectionSucc[i], _positions[i + 1] - _positions[i]) * Quaternion.Inverse(_startRotationBone[i]));
	            SetPositionRootSpace(_bones[i].Transform, _positions[i]);
            }
		}

		private Vector3 GetPositionRootSpace(Transform current)
		{
			if (_root == null)
				return current.position;
			return Quaternion.Inverse(_root.Transform.rotation) * (current.position - _root.Transform.position);
		}

		private void SetPositionRootSpace(Transform current, Vector3 position)
		{
			if (_root == null)
				current.position = position;
			else
				current.position = _root.Transform.rotation * position + _root.Transform.position;
		}

		private Quaternion GetRotationRootSpace(Transform current)
		{
			if (_root == null)
				return current.rotation;
			return Quaternion.Inverse(current.rotation) * _root.Transform.rotation;
		}

		private void SetRotationRootSpace(Transform current, Quaternion rotation)
		{
			if (_root == null)
				current.rotation = rotation;
			else
				current.rotation = _root.Transform.rotation * rotation;
		}

		private void CreateNewPole(Animator animator, IReadOnlyDictionary<HumanBodyBones, HumanoidFabrikEffector> dictionary)
		{
			_pole = new GameObject("Pole" + EndEffector.Bone).transform;
			_pole.SetParent(animator.transform, false);

			var tr      = animator.transform;
			var forward = tr.forward;
			var right   = tr.right;
			switch (EndEffector.Bone)
			{
				case HumanBodyBones.LeftFoot:
					var leftLowerLeg = dictionary[HumanBodyBones.LeftLowerLeg];
					_pole.position = leftLowerLeg.Transform.position + forward;
					break;
				case HumanBodyBones.RightFoot:
					var rightLowerLeg = dictionary[HumanBodyBones.RightLowerLeg];
					_pole.position = rightLowerLeg.Transform.position + forward;
					break;
				case HumanBodyBones.LeftHand:
					var leftLowerArm = dictionary[HumanBodyBones.LeftLowerArm];
					_pole.position = leftLowerArm.Transform.position + -right;
					break;
				case HumanBodyBones.RightHand:
					var rightLowerArm = dictionary[HumanBodyBones.RightLowerArm];
					_pole.position = rightLowerArm.Transform.position + right;
					break;
			}
		}
    }
}
