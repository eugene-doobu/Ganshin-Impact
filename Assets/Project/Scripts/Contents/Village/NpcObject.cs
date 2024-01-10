using GanShin.GanObject;
using GanShin.Village.Base;
using UnityEngine;

namespace GanShin.Village.Contents
{
    public class NpcObject : StaticObject, ITriggerEventProvider
    {
        [field: SerializeField] public ENpcType NpcType { get; set; }

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