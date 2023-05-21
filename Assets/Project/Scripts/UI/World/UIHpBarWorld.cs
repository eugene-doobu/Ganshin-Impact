using UnityEngine;

namespace GanShin.UI
{
    /// <summary>
    /// 해당 오브젝트는 Canvas에 부착하고, 부모는 오브젝트의 콜라이더를 가지고 있다고 가정합니다.
    /// </summary>
    public class UIHpBarWorld : UIHpBar
    {
        [SerializeField] private float scaleFactor = 350.0f;
        
        private void Update()
        {
            var tr         = transform;
            var mainCamera = Camera.main;
            
            var parent = tr.parent;
            tr.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

            if (mainCamera == null)
                return;
            
            tr.rotation = mainCamera.transform.rotation;
            
            float camHeight;
            if (mainCamera.orthographic)
            {
                camHeight = mainCamera.orthographicSize * 2;
            }
            else
            {
                var distanceToCamera = Vector3.Distance(mainCamera.transform.position, transform.position);
                camHeight = 2.0f * distanceToCamera * Mathf.Tan(Mathf.Deg2Rad * (mainCamera.fieldOfView * 0.5f));
            }
            var scale = (camHeight / Screen.width) * scaleFactor;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}