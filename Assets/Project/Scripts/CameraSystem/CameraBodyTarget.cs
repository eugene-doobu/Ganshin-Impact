using System;
using System.Collections;
using System.Collections.Generic;
using GanShin.Utils;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    public class CameraBodyTarget : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private Transform? _target;

        [SerializeField] private float _tracingTargetSmoothFactor = 6f;
        [SerializeField] private float _rotationSmoothFactor      = 11f;

        [SerializeField] private float _moveImmdiateSqrDistance = 70f;

        private Transform? _tr;

        private void Awake()
        {
            _tr = GetComponent<Transform>();
        }

        private void Update()
        {
            if (_target == null) return;
            if (Vector3.SqrMagnitude(_target.position - _tr!.position) > _moveImmdiateSqrDistance)
            {
                SetPositionImmediate();
                return;
            }

            _tr!.position = Vector3.Lerp(_tr.position, _target.position, _tracingTargetSmoothFactor * Time.deltaTime);
        }

        public void SetPositionImmediate()
        {
            if (_target == null) return;
            SetPositionImmediate(_target.position);
        }

        public void SetPositionImmediate(Vector3 position)
        {
            _tr!.position = position;
        }

        public void SetRotation(float angle)
        {
            var targetRotation = Quaternion.Euler(0, angle, 0);
            _tr!.rotation = Quaternion.Slerp(_tr.rotation, targetRotation, _rotationSmoothFactor * Time.deltaTime);
        }

        public void SetRotation(Quaternion angle)
        {
            _tr!.rotation = Quaternion.Slerp(_tr.rotation, angle, _rotationSmoothFactor * Time.deltaTime);
        }

        public void SetTarget(Transform? target)
        {
            if (ReferenceEquals(target, _target)) return;
            _target = target;
        }
    }
}