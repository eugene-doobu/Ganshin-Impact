using UnityEngine;

namespace GanShin.UI
{
    /// <summary>
    /// 해당 오브젝트는 Canvas에 부착하고, 부모는 오브젝트의 콜라이더를 가지고 있다고 가정합니다.
    /// </summary>
    public class UIHpBarWorld : UIHpBar
    {
        private void Update()
        {
            var tr = transform;
            
            var parent = tr.parent;
            tr.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
            tr.rotation = Camera.main.transform.rotation;
        }
    }
}