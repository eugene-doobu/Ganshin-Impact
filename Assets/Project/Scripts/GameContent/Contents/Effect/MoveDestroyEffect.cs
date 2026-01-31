using UnityEngine;

namespace GanShin.Effect
{
    public class MoveDestroyEffect : MonoBehaviour
    {
        [SerializeField] private GameObject gameObjectTail;
        [SerializeField] private Transform  hitPrefab;

        [SerializeField] private float hitRayMaxLength;
        [SerializeField] private bool  isDestroyObject;
        [SerializeField] private float destroyTime;
        [SerializeField] private float tailDestroyTime;
        [SerializeField] private float hitObjectDestroyTime;
        [SerializeField] private float moveSpeed;
        
        private GameObject _hitObject;
        
        private float _time;
        private bool _isHit;

        private void Start()
        {
            _time = Time.time;
        }

        private void LateUpdate()
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));
            if (!_isHit)
            {
                if (Physics.Raycast(transform.position, transform.forward, out var hit, hitRayMaxLength))
                    HitObj(hit);
            }

            if (!isDestroyObject) return;
            if (!(Time.time > _time + destroyTime)) return;

            MakeHitObject(transform);
            DestroyObjects();
        }    
        
        private void MakeHitObject(RaycastHit hit)
        {
            if (hitPrefab == null)
                return;

            _hitObject = Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
            _hitObject.transform.parent = transform.parent;
            _hitObject.transform.localScale = new Vector3(1, 1, 1);
        }

        private void MakeHitObject(Transform point)
        {
            if (hitPrefab == null)
                return;
        
            _hitObject = Instantiate(hitPrefab, point.transform.position, point.rotation).gameObject;
            _hitObject.transform.parent = transform.parent;
            _hitObject.transform.localScale = new Vector3(1, 1, 1);
        }

        private void HitObj(RaycastHit hit)
        {
            _isHit = true;
            if (gameObjectTail != null)
                gameObjectTail.transform.parent = null;
            MakeHitObject(hit);

            DestroyObjects();
        }

        private void DestroyObjects()
        {
            Destroy(gameObject);
            Destroy(gameObjectTail, tailDestroyTime);
            Destroy(_hitObject, hitObjectDestroyTime);
        }
    }
}
