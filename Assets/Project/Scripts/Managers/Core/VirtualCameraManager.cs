using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.CameraSystem
{
    public class VirtualCameraManager
    {
        private Dictionary<string, VirtualCameraJig> _virtualCameraDict = new();

        public void AddVirtualCamera(VirtualCameraJig jig)
        {
            _virtualCameraDict[jig.Name] = jig;
        }
        
        public void RemoveVirtualCamera(VirtualCameraJig jig)
        {
            if (!_virtualCameraDict.ContainsKey(jig.Name))
            {
                Debug.LogError($"[{nameof(VirtualCameraManager)}] RemoveVirtualCamera: jig not found");
            }
            _virtualCameraDict.Remove(jig.Name);
        }
    }
}
