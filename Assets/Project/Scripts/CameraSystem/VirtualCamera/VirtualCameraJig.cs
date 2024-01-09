using System;
using Cinemachine;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraJig : MonoBehaviour
    {
        private CameraManager? Camera => ProjectManager.Instance.GetManager<CameraManager>();

        [field: SerializeField] public string Name { get; private set; } = String.Empty;

        private CinemachineVirtualCamera? _virtualCamera;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (string.IsNullOrWhiteSpace(Name)) Name = gameObject.name;
        }

        private void OnEnable()
        {
            if (Camera == null) return;
            Camera.AddVirtualCamera(this);
        }

        private void OnDisable()
        {
            if (Camera == null) return;
            Camera.RemoveVirtualCamera(this);
        }
    }
}