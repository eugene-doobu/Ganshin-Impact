using UnityEngine;

namespace GanShin.Entities
{
    public interface ITriggerEventProvider
    {
        void OnTriggerEnter(Collider other);

        void OnTriggerExit(Collider other);
    }
}