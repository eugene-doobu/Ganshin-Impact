#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace GanShin.FABRIK
{
    public class FabrikChain
    {
#region Fields
	    private readonly List<HumanoidFabrikEffector> _effectors;

	    private Vector3 _targetPosition;
	    private Quaternion _targetRotation;
	    private bool _hasTarget;

	    private const int   Iterations       = 10;
	    private const float Delta            = 0.001f;
	    private const float SnapBackStrength = 1f;
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
		    get => _targetPosition;
		    set
		    {
			    this.Target.position = value;
			    _targetPosition = value;
			    _hasTarget = true;
		    }
	    }

	    public Quaternion TargetRotation
	    {
		    get => _targetRotation;
		    set
		    {
			    this.Target.rotation = value;
			    _targetRotation = value;
			    _hasTarget = true;
		    }
	    }

	    public int Layer { get; private set; }

	    private HumanoidFabrikEffector BaseEffector => _effectors[0];
	    public  HumanoidFabrikEffector EndEffector  => _effectors[^1];
#endregion Property

	    protected float[]      BonesLength; //Target to Origin
	    protected float        CompleteLength;
	    protected Transform[]  Bones;
	    protected Vector3[]    Positions;
	    protected Vector3[]    StartDirectionSucc;
	    protected Quaternion[] StartRotationBone;
	    protected Quaternion   StartRotationTarget;
	    protected Transform    Root;

	    public Transform Target;
	    public Transform Pole;

		public FabrikChain(List<HumanoidFabrikEffector> effectors, int layer, Animator animator, IReadOnlyDictionary<HumanBodyBones, HumanoidFabrikEffector> dictionary)
		{
			_effectors = new List<HumanoidFabrikEffector>(effectors);

			Layer = layer;

			var ChainLength = _effectors.Count - 2;
			if (ChainLength < 1)
				return;

			//initial array
			Bones              = new Transform[ChainLength + 1];
			Positions          = new Vector3[ChainLength + 1];
			BonesLength        = new float[ChainLength];
			StartDirectionSucc = new Vector3[ChainLength + 1];
			StartRotationBone  = new Quaternion[ChainLength + 1];

			//find root
			Root = EndEffector.Transform;
			for (var i = 0; i <= ChainLength; i++)
			{
				if (Root == null)
					throw new UnityException("The chain value is longer than the ancestor chain!");
				Root = Root.parent;
			}

			//init target
			if (Target == null)
			{
				Target = new GameObject("Target").transform;
				SetPositionRootSpace(Target, GetPositionRootSpace(EndEffector.Transform));
			}
			StartRotationTarget = GetRotationRootSpace(Target);

			if (Pole == null || EndEffector.Bone != HumanBodyBones.Head)
			{
				Pole = new GameObject("Pole" + EndEffector.Bone).transform;
				Pole.SetParent(animator.transform, false);
				switch (EndEffector.Bone)
				{
					case HumanBodyBones.LeftFoot:
						var leftLowerLeg = dictionary[HumanBodyBones.LeftLowerLeg];
						Pole.position = leftLowerLeg.Transform.position + animator.transform.forward;
						break;
					case HumanBodyBones.RightFoot:
						var rightLowerLeg = dictionary[HumanBodyBones.RightLowerLeg];
						Pole.position = rightLowerLeg.Transform.position + animator.transform.forward;
						break;
					case HumanBodyBones.LeftHand:
						var leftLowerArm = dictionary[HumanBodyBones.LeftLowerArm];
						Pole.position = leftLowerArm.Transform.position + animator.transform.forward;
						break;
					case HumanBodyBones.RightHand:
						var rightLowerArm = dictionary[HumanBodyBones.RightLowerArm];
						Pole.position = rightLowerArm.Transform.position + animator.transform.forward;
						break;
				}
			}

			//init data
			var current = EndEffector;
			CompleteLength = 0;
			for (var i = Bones.Length - 1; i >= 0; i--)
			{
				Bones[i]             = current.Transform;
				StartRotationBone[i] = GetRotationRootSpace(current.Transform);

				if (i == Bones.Length - 1)
				{
					//leaf
					StartDirectionSucc[i] = GetPositionRootSpace(Target) - GetPositionRootSpace(current.Transform);
				}
				else
				{
					//mid bone
					StartDirectionSucc[i] =  GetPositionRootSpace(Bones[i + 1]) - GetPositionRootSpace(current.Transform);
					BonesLength[i]        =  StartDirectionSucc[i].magnitude;
					CompleteLength        += BonesLength[i];
				}

				var parent = HumanoidUtils.GetParentBone(current.Bone);
				if (parent == HumanBodyBones.LastBone)
					break;
				GanDebugger.Log("Bone: " + current.Bone + " Parent: " + parent);
				current = dictionary[parent];
			}
		}

		public void SolveIK()
		{
			if (!_hasTarget)
				return;

			if (Target == null)
				return;

            //Fabric

            //  root
            //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
            //   x--------------------x--------------------x---...

            //get position
            for (int i = 0; i < Bones.Length; i++)
                Positions[i] = GetPositionRootSpace(Bones[i]);

            var targetPosition = GetPositionRootSpace(Target);
            var targetRotation = GetRotationRootSpace(Target);

            //1st is possible to reach?
            if ((targetPosition - GetPositionRootSpace(Bones[0])).sqrMagnitude >= CompleteLength * CompleteLength)
            {
	            //just strech it
	            var direction = (targetPosition - Positions[0]).normalized;
	            //set everything after root
	            for (int i = 1; i < Positions.Length; i++)
		            Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
            }
            else
            {
	            for (int i = 0; i < Positions.Length - 1; i++)
		            Positions[i + 1] = Vector3.Lerp(Positions[i + 1], Positions[i] + StartDirectionSucc[i], SnapBackStrength);

                for (int iteration = 0; iteration < Iterations; iteration++)
                {
                    //https://www.youtube.com/watch?v=UNoX65PRehA
                    //back
                    for (int i = Positions.Length - 1; i > 0; i--)
                    {
                        if (i == Positions.Length - 1)
                            Positions[i] = targetPosition; //set it to target
                        else
                            Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i]; //set in line on distance
                    }

                    //forward
                    for (int i = 1; i < Positions.Length; i++)
                        Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];

                    //close enough?
                    if ((Positions[Positions.Length - 1] - targetPosition).sqrMagnitude < Delta * Delta)
                        break;
                }
            }

            //move towards pole
            if (Pole != null)
            {
                var polePosition = GetPositionRootSpace(Pole);
                for (int i = 1; i < Positions.Length - 1; i++)
                {
                    var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                    var projectedPole = plane.ClosestPointOnPlane(polePosition);
                    var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                    var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                    Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
                }
            }

            //set position & rotation
            for (int i = 0; i < Positions.Length; i++)
            {
	            if (i == Positions.Length - 1)
		            SetRotationRootSpace(Bones[i], Quaternion.Inverse(targetRotation) * StartRotationTarget * Quaternion.Inverse(StartRotationBone[i]));
	            else
		            SetRotationRootSpace(Bones[i], Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * Quaternion.Inverse(StartRotationBone[i]));
	            SetPositionRootSpace(Bones[i], Positions[i]);
            }
		}

		private Vector3 GetPositionRootSpace(Vector3 position)
		{
			if (Root == null)
				return position;
			else
				return Quaternion.Inverse(Root.rotation) * (position - Root.position);
		}

		private Vector3 GetPositionRootSpace(Transform current)
		{
			if (Root == null)
				return current.position;
			else
				return Quaternion.Inverse(Root.rotation) * (current.position - Root.position);
		}

		private void SetPositionRootSpace(Transform current, Vector3 position)
		{
			if (Root == null)
				current.position = position;
			else
				current.position = Root.rotation * position + Root.position;
		}

		private Quaternion GetRotationRootSpace(Quaternion rotation)
		{
			if (Root == null)
				return rotation;
			else
				return Quaternion.Inverse(Root.rotation) * rotation;
		}

		private Quaternion GetRotationRootSpace(Transform current)
		{
			//inverse(after) * before => rot: before -> after
			if (Root == null)
				return current.rotation;
			else
				return Quaternion.Inverse(current.rotation) * Root.rotation;
		}

		private void SetRotationRootSpace(Transform current, Quaternion rotation)
		{
			if (Root == null)
				current.rotation = rotation;
			else
				current.rotation = Root.rotation * rotation;
		}
    }
}
