using System.Runtime.CompilerServices;
using GanShin.Utils;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    public class CameraBodyTarget : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private Transform? _target;

        [SerializeField] private float _tracingTargetSmoothFactor = 12f;
        [SerializeField] private float _rotationSmoothFactor      = 11f;

        [SerializeField] private float _yOffset = 1.32f;
        [SerializeField] private float _moveImmdiateSqrDistance = 5f;

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

            _tr!.position = Vector3.Lerp(_tr.position, GetTargetPosition(_target.position), _tracingTargetSmoothFactor * Time.deltaTime);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetTargetPosition(Vector3 targetPosition) =>
             new(targetPosition.x, targetPosition.y + _yOffset, targetPosition.z);

        public void SetPositionImmediate()
        {
            if (_target == null) return;
            SetPositionImmediate(_target.position);
        }

        public void SetPositionImmediate(Vector3 position)
        {
            _tr!.position = GetTargetPosition(position);
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