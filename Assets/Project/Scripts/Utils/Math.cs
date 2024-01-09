using UnityEngine;

namespace GanShin
{
    public static class MathUtils
    {
        public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle  -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}