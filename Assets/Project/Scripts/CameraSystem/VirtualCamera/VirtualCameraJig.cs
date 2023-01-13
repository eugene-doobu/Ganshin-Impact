using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

#nullable enable

namespace GanShin.CameraSystem
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraJig : MonoBehaviour
    {
        [Inject] private CameraManager? _camera;
        
        [field:SerializeField] public string Name { get; private set; } = String.Empty;
        
        private CinemachineVirtualCamera? _virtualCamera;
        
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (string.IsNullOrWhiteSpace(Name)) Name = gameObject.name;
        }

        private void OnEnable()
        {
            if (_camera == null) return;
            _camera.AddVirtualCamera(this);
        }

        private void OnDisable()
        {
            if (_camera == null) return;
            _camera.RemoveVirtualCamera(this);
        }
    }
}
