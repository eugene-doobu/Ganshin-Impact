using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace GanShin.CameraSystem
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraJig : MonoBehaviour
    {
        [field:SerializeField] public string Name { get; private set; }
        
        private CinemachineVirtualCamera _virtualCamera;
        
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (string.IsNullOrWhiteSpace(Name)) Name = gameObject.name;
        }

        private void OnEnable()
        {
            Managers.Camera.AddVirtualCamera(this);
        }

        private void OnDisable()
        {
            Managers.Camera.RemoveVirtualCamera(this);
        }
    }
}
