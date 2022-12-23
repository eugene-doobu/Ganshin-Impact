using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

#nullable enable

namespace GanShin.CameraSystem
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraJig : MonoBehaviour
    {
        [field:SerializeField] public string Name { get; private set; } = String.Empty;
        
        private CinemachineVirtualCamera? _virtualCamera;
        
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (string.IsNullOrWhiteSpace(Name)) Name = gameObject.name;
        }

        private void OnEnable()
        {
            if (!Managers.InstanceExist) return;
            if (Managers.Camera == null) return;
            Managers.Camera.AddVirtualCamera(this);
        }

        private void OnDisable()
        {
            if (!Managers.InstanceExist) return;
            Managers.Camera.RemoveVirtualCamera(this);
        }
    }
}
