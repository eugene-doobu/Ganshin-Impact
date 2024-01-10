#nullable enable

using Cinemachine;
using UnityEngine;

namespace GanShin.CameraSystem
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraJig : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; } = string.Empty;

        private CinemachineVirtualCamera? _virtualCamera;
        private CameraManager?            Camera => ProjectManager.Instance.GetManager<CameraManager>();

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