using UnityEngine;

namespace GanShin.GanObject
{
    /// <summary>
    /// AI를 가지며 스스로의 판단으로 움직이는 오브젝트들
    /// 플레이어와 몬스터가 이 클래스에 속함
    /// </summary>
    public class CreatureObject : Actor
    {
#region Variables
        private Animator _objAnimator;
#endregion Variables

#region Properties
        protected Animator ObjAnimator => _objAnimator;
        protected bool     HasAnimator { get; private set; }
        
        protected bool IsInitialized { get; private set; }
#endregion Properties

#region Mono
        // TODO: MapController의 Init로 변경
        protected override void Awake()
        {
            base.Awake();
            // Unity Null체크의 비용은 무겁기 때문에 미리 체크한 후 결과를 캐싱하여 사용
            HasAnimator = TryGetComponent(out _objAnimator);
        }

        public override void Tick()
        {
        }
#endregion Mono
    }
}