using UnityEngine;

namespace GanShin.Utils
{
    public class FixWorldRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;
        
        private void Update()
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
