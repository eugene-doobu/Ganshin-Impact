using System.Collections.Generic;
using UnityEngine;

namespace GanShin.FABRIK
{
    public class HumanoidFabrik
    {
	    private FabrikChain _rootChain;

		private readonly Dictionary<HumanBodyBones, HumanoidFabrikEffector> _effectors = new();

		private readonly Dictionary<HumanBodyBones, FabrikChain> _endChains = new();

		private readonly List<FabrikChain> _chains = new();

		public void Initialize(Animator animator)
		{
			InitializeEffectors(animator);
			var rootEffector = _effectors[HumanBodyBones.Hips];

			_rootChain = LoadSystem(rootEffector);
			_chains.Sort((x, y) => y.Layer.CompareTo(x.Layer));

			foreach(var chain in _chains)
				chain.CalculateSummedWeight();
		}

		private void InitializeEffectors(Animator animator)
		{
			_effectors.Clear();

			foreach (var ikEffectorBone in HumanoidUtils.IkEffectorBones)
			{
				var targetTransform = animator.GetBoneTransform(ikEffectorBone);
				var parentBone      = HumanoidUtils.GetParentBone(ikEffectorBone);
				var parentEffector  = parentBone == HumanBodyBones.LastBone ? null : _effectors[parentBone];
				_effectors[ikEffectorBone] = new HumanoidFabrikEffector(ikEffectorBone, targetTransform, parentEffector);
			}
		}

		private FabrikChain LoadSystem(HumanoidFabrikEffector effector, FabrikChain parent = null, int layer = 0)
		{
			var effectors = new List<HumanoidFabrikEffector>();
			if (parent != null)
				effectors.Add(parent.EndEffector);

			List<HumanBodyBones> childrenBones = null;
			while (effector != null)
			{
				childrenBones = HumanoidUtils.GetChildrenBones(effector.Bone);
				effectors.Add(effector);
				if (childrenBones == null)
					break;

				// childCount > 1 is a new sub-base
				if (childrenBones.Count != 1)
					break;

				effector = _effectors[childrenBones[0]];
			}

			var chain = new FabrikChain(parent, effectors, layer);
			_chains.Add(chain);

			if (chain.IsEndChain)
			{
				if (childrenBones is { Count: > 0 } && _effectors.TryGetValue(childrenBones[0], out var childEffector))
					_endChains.Add(childEffector.Bone, chain);
			}
			else if (childrenBones != null)
				foreach (var child in childrenBones)
					LoadSystem(_effectors[child], chain, layer + 1);

			return chain;
		}

		public void Solve()
		{
			foreach (var chain in _chains)
				chain.Backward();

			_rootChain.ForwardMulti();
		}

		public void SetTarget(AvatarIKGoal avatarIKGoal, Vector3 target)
		{
			var bone = HumanBodyBones.LastBone;
			switch (avatarIKGoal)
			{
				case AvatarIKGoal.LeftFoot:
					bone = HumanBodyBones.LeftToes;
					break;
				case AvatarIKGoal.RightFoot:
					bone = HumanBodyBones.RightToes;
					break;
				case AvatarIKGoal.LeftHand:
					bone = HumanBodyBones.LeftHand;
					break;
				case AvatarIKGoal.RightHand:
					bone = HumanBodyBones.RightHand;
					break;
			}

			if (!_endChains.TryGetValue(bone, out var chain))
				return;

			chain.Target = target;
		}

		public void SetTarget(HumanBodyBones bone, Vector3 target)
		{
			if (!_endChains.TryGetValue(bone, out var chain))
				return;

			chain.Target = target;
		}
    }
}
