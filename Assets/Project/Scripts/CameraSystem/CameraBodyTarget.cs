using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    public class CameraBodyTarget : MonoBehaviour
    {
        // TODO: Readonly
        [SerializeField] private Transform? _target;
        [SerializeField] private float      _tracingTargetSmoothFactor = 6f;

        private Transform? _tr;
        
        private void Awake()
        {
            _tr = GetComponent<Transform>();
        }

        private void Start()
        {
            if (_target == null)
            {
                GanDebugger.CameraLogError("CameraBodyTarget - Target is null");
                return;
            }
            SetPositionImmediate(_target.position);
        }

        private void Update()
        {
            if (_target == null) return;
            _tr!.position = Vector3.Lerp(_tr.position, _target.position, _tracingTargetSmoothFactor * Time.deltaTime);
        }
        
        public void SetPositionImmediate(Vector3 position)
        {
            _tr!.position = position;
        }

        public void SetRotation(float angle)
        {
            _tr!.rotation = Quaternion.Euler(0, angle, 0);
        }

        public void SetTarget(Transform target)
        {
            if(ReferenceEquals(target, _target)) return;
            _target = target;
        }
    }
}
    