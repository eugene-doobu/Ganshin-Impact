using GanShin.MapObject;
using UnityEngine;

namespace GanShin.Village.Contents
{
    public class NpcObject : StaticObject, ITriggerEventProvider
    {
        public void OnTriggerEnter(Collider other)
        {
            GanDebugger.LogWarning("Enter");
        }

        public void OnTriggerExit(Collider other)
        {
            GanDebugger.LogWarning("Exit");
        }
    }
}
