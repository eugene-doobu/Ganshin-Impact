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

        public static float GetRandomValue(float value)
        {
            return Random.Range(-value, value);
        }    
        
        public static float GetRandomUnsignedValue(float value)
        {
            return Random.Range(0, value);
        }

        public static Vector3 GetRandomVector(Vector3 value)
        {
            Vector3 result;
            result.x = GetRandomValue(value.x);
            result.y = GetRandomValue(value.y);
            result.z = GetRandomValue(value.z);
            return result;
        }

        public static Vector3 GetRandomUnsignedVector(Vector3 value)
        {
            Vector3 result;
            result.x = GetRandomUnsignedValue(value.x);
            result.y = GetRandomUnsignedValue(value.y);
            result.z = GetRandomUnsignedValue(value.z);
            return result;
        }
    }
}