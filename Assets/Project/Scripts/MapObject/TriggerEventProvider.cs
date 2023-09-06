using UnityEngine;

namespace GanShin.MapObject
{
    public interface ITriggerEventProvider
    {
        void OnTriggerEnter(Collider other);

        void OnTriggerExit(Collider other);
    }
}
