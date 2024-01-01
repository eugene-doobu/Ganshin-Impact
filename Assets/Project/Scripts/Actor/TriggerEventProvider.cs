using UnityEngine;

namespace GanShin.GanObject
{
    public interface ITriggerEventProvider
    {
        void OnTriggerEnter(Collider other);

        void OnTriggerExit(Collider other);
    }
}
