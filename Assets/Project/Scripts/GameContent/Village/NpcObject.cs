using GanShin.Entities;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.Village
{
    public class NpcObject : PassiveObject, ITriggerEventProvider
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