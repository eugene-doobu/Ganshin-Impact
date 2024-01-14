using UnityEngine;

namespace GanShin
{
    public static class DebuggingUtil
    {
        #if UNITY_EDITOR
        public static void DrawCapsule(Vector3 pos, Vector3 dir, float radius, float height, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(pos + dir * (height * 0.5f - radius), radius);
            Gizmos.DrawWireSphere(pos - dir * (height * 0.5f - radius), radius);
            Gizmos.DrawLine(pos + dir * (height * 0.5f - radius), pos - dir * (height * 0.5f - radius));
        }
        #endif // UNITY_EDITOR
    }
}