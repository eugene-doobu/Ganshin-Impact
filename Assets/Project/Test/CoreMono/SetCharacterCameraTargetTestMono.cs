using System.Collections;
using System.Collections.Generic;
using GanShin;
using UnityEngine;

namespace GanShinTest
{
    public class SetCharacterCameraTargetTestMono : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        void Update()
        {
            if (Input.GetKeyDown("]") && _target != null)
                Managers.Camera.ChangeTarget(_target);
            
            if (Input.GetKeyDown("["))
                Managers.Camera.ChangeTarget(null);
        }
    }
}
