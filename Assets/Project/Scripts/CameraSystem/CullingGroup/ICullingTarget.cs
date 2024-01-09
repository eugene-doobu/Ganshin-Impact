using UnityEngine;

namespace GanShin.CameraSystem
{
	/// <summary>
	///     CullingGroup의 Transform을 언제 업데이트할지 결정합니다
	/// </summary>
	public enum eCullingUpdateMode
    {
	    /// <summary>
	    ///     CullingGroup의 Transform을 항상 갱신합니다
	    /// </summary>
	    DYNAMIC = 0,

	    /// <summary>
	    ///     CullingGroup의 Transform을 첫번째 업데이트나 수동으로 업데이트 요청을 보낸 경우 갱신합니다.
	    /// </summary>
	    STATIC = 1
    }

    public interface ICullingTarget
    {
        CullingGroupProxy HudCullingGroup { get; set; }

        eCullingUpdateMode BoundingSphereUpdateMode { get; }

        /// <summary>
        ///     마지막으로 업데이트된 Bounding Sphere를 리턴합니다
        /// </summary>
        BoundingSphere BoundingSphere { get; }

        /// <summary>
        ///     거리에 따라 컬링그룹 상태가 변경될 때 호출될 콜백을 등록합니다
        /// </summary>
        CullingGroup.StateChanged OnHudStateChanged { get; }

        /// <summary>
        ///     Bounding Sphere를 업데이트 후 리턴합니다
        /// </summary>
        /// <returns></returns>
        BoundingSphere UpdateAndGetBoundingSphere();
    }
}