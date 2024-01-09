using UnityEngine;

namespace GanShin.Content.Creature
{
    public abstract class CreatureController : MonoBehaviour
    {
#region Variables

        private Animator _objAnimator;

#endregion Variables

#region Properties

        protected Animator ObjAnimator => _objAnimator;
        protected bool     HasAnimator { get; private set; }

#endregion Properties

#region Mono

        // TODO: MapController의 Init로 변경
        protected virtual void Awake()
        {
            // Unity Null체크의 비용은 무겁기 때문에 미리 체크한 후 결과를 캐싱하여 사용
            HasAnimator = TryGetComponent(out _objAnimator);
        }

        protected virtual void Start()
        {
        }

        // TODO: MapController의 OnUpdate로 변경
        protected virtual void Update()
        {
        }

#endregion Mono
    }
}