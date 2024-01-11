#nullable enable

using System;
using System.Collections.Generic;
using GanShin.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GanShin.CameraSystem
{
    public enum eCullingGroupType
    {
        OBJECT_HUD = 0
    }

    public enum eCullingTargetUpdateMode
    {
        EVERY_UPDATE = 0,
        MANUAL       = 1
    }

    [RequireComponent(typeof(Camera))]
    public sealed class CullingGroupProxy : MonoBehaviour
    {
        public void Add(ICullingTarget target)
        {
            _targetsToAdd.Add(target);
            _targetsToRemove.Remove(target);
        }

        public void Remove(ICullingTarget target)
        {
            _targetsToRemove.Add(target);
            _targetsToAdd.Remove(target);
        }

        public bool IsVisible(ICullingTarget target)
        {
            var index = IndexOf(target);
            if (index == -1)
                return false;

            return _cullingGroup!.IsVisible(index);
        }

        public bool IsContained(ICullingTarget target)
        {
            return _cullingTargets.Contains(target);
        }

        public int IndexOf(ICullingTarget target)
        {
            return _cullingTargets.IndexOf(target);
        }

        public void UpdateDynamicBoundingSphereTransforms()
        {
            foreach (var index in _dynamicTargetIndices)
            {
                var target = _cullingTargets[index];
                if (target is Object)
                    _boundingSpheres[index] = target.UpdateAndGetBoundingSphere();
            }
        }

        public void UpdateAllBoundingSphereTransforms()
        {
            for (var i = 0; _cullingTargets.Count > i; i++)
            {
                var target = _cullingTargets[i];
                _boundingSpheres[i] = target.UpdateAndGetBoundingSphere();
            }
        }

        private void OnStateChanged(CullingGroupEvent cullingGroupEvent)
        {
            _cullingTargets[cullingGroupEvent.index].OnHudStateChanged?.Invoke(cullingGroupEvent);
        }

        public void SetCullingGroupType(eCullingGroupType cullingGroupType)
        {
            _cullingGroupType = cullingGroupType;
            var boundingDistances = GetCullingGroupBoundingDistance(cullingGroupType);
            BoundingDistances = boundingDistances ?? DefaultDistances;
        }

        private static float[]? GetCullingGroupBoundingDistance(eCullingGroupType cullingGroupType)
        {
            return cullingGroupType switch
            {
                eCullingGroupType.OBJECT_HUD => new[] { 0f, 4, 8, 14 },
                _                            => null
            };
        }

#region ConstantFields
        private static readonly int     ArrayMinLength   = 16;
        private static readonly float[] DefaultDistances = { 0, float.PositiveInfinity };

        private readonly List<ICullingTarget>    _cullingTargets       = new();
        private readonly HashSet<ICullingTarget> _targetsToAdd         = new();
        private readonly HashSet<ICullingTarget> _targetsToRemove      = new();
        private readonly List<int>               _dynamicTargetIndices = new();
#endregion ConstantFields

#region InspectorFields
        [SerializeField] [ReadOnly] private eCullingGroupType _cullingGroupType = eCullingGroupType.OBJECT_HUD;

        [SerializeField] private eCullingTargetUpdateMode _targetsUpdateMode = eCullingTargetUpdateMode.EVERY_UPDATE;

        [SerializeField] private Transform? _distanceReferencePoint;

        [SerializeField] private float[] _boundingDistances = Array.Empty<float>();
#endregion InspectorFields

#region Fields
        private BoundingSphere[] _boundingSpheres = Array.Empty<BoundingSphere>();
        private CullingGroup?    _cullingGroup;
#endregion Fields

#region Properites
        public Transform? DistanceReferencePoint
        {
            get => _distanceReferencePoint;
            set
            {
                _distanceReferencePoint = value;
                _cullingGroup?.SetDistanceReferencePoint(_distanceReferencePoint);
            }
        }

        public float[] BoundingDistances
        {
            get => _boundingDistances;
            set
            {
                _boundingDistances = value;
                _cullingGroup?.SetBoundingDistances(_boundingDistances.Length > 0
                                                        ? _boundingDistances
                                                        : DefaultDistances);
            }
        }

        public IReadOnlyList<ICullingTarget> Targets => _cullingTargets;

        public IReadOnlyList<BoundingSphere> BoundingSpheres => _boundingSpheres;

        public eCullingGroupType CullingGroupType => _cullingGroupType;
#endregion Properites

#region MonoBehaviour
        private void Awake()
        {
            _cullingGroup = new CullingGroup
            {
                targetCamera   = GetComponent<Camera>(),
                onStateChanged = OnStateChanged
            };
        }

        private void Start()
        {
            if (_cullingGroup == null) return;
            _cullingGroup.SetBoundingDistances(_boundingDistances.Length > 0 ? _boundingDistances : DefaultDistances);
            _cullingGroup.SetDistanceReferencePoint(DistanceReferencePoint);
        }

        private void OnEnable()
        {
            if (_cullingGroup == null)
            {
                GanDebugger.LogWarning(GetType().Name, "CullingGroup is null");
                return;
            }

            _cullingGroup.enabled = true;
        }

        private void OnDisable()
        {
            if (_cullingGroup == null)
            {
                GanDebugger.LogWarning(GetType().Name, "CullingGroup is null");
                return;
            }

            _cullingGroup.enabled = false;
        }

        private void OnDestroy()
        {
            _dynamicTargetIndices.Clear();
            _cullingGroup?.Dispose();
            _cullingGroup = null;
        }

        private void Update()
        {
            if (_targetsUpdateMode == eCullingTargetUpdateMode.EVERY_UPDATE) UpdateTargets();

            UpdateDynamicBoundingSphereTransforms();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            DistanceReferencePoint = DistanceReferencePoint;
            BoundingDistances      = BoundingDistances;
        }
#endif
#endregion MonoBehaviour

#region UpdateTarget
        public void UpdateTargets()
        {
            var hasTargetsToRemove = _targetsToRemove.Count > 0;
            if (hasTargetsToRemove) RemoveTargets();

            var hasTargetsToAdd = _targetsToAdd.Count > 0;
            if (hasTargetsToAdd) AddTargets();

            if (hasTargetsToAdd || hasTargetsToRemove)
            {
                var targetsCount = _cullingTargets.Count;

                var nextPowerOfTwo = Mathf.NextPowerOfTwo(targetsCount);
                var length         = nextPowerOfTwo > ArrayMinLength ? nextPowerOfTwo : ArrayMinLength;
                if (length != _boundingSpheres.Length)
                    _boundingSpheres = new BoundingSphere[length];

                _dynamicTargetIndices.Clear();
                for (var i = 0; targetsCount > i; i++)
                {
                    var target = _cullingTargets[i];
                    if (target == null) continue;
                    _boundingSpheres[i] = target.UpdateAndGetBoundingSphere();

                    if (target.BoundingSphereUpdateMode == eCullingUpdateMode.DYNAMIC)
                        _dynamicTargetIndices.Add(i);
                }

                _cullingGroup!.SetBoundingSphereCount(0);
                _cullingGroup.SetBoundingSpheres(_boundingSpheres);
                _cullingGroup.SetBoundingSphereCount(targetsCount);
            }
        }

        private void RemoveTargets()
        {
            foreach (var target in _targetsToRemove)
                _cullingTargets.Remove(target);
            _targetsToRemove.Clear();
        }

        private void AddTargets()
        {
            _cullingTargets.AddRange(_targetsToAdd);
            _targetsToAdd.Clear();
        }
#endregion UpdateTarget
    }
}