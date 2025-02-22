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

        public static void Decompose(this Quaternion quaternion, Vector3 direction, out Quaternion swing, out Quaternion twist)
        {
            var vector     = new Vector3(quaternion.x, quaternion.y, quaternion.z);
            var projection = Vector3.Project(vector, direction);

            twist = new Quaternion(projection.x, projection.y, projection.z, quaternion.w);
            twist.Normalize();
            swing = quaternion * Quaternion.Inverse(twist);
        }

        public static Quaternion Constrain(this Quaternion quaternion, float angle)
        {
            var magnitude    = Mathf.Sin(0.5f * angle);
            var sqrMagnitude = magnitude * magnitude;

            var vector = new Vector3(quaternion.x, quaternion.y, quaternion.z);

            if (!(vector.sqrMagnitude > sqrMagnitude)) return quaternion;

            vector = vector.normalized * magnitude;

            quaternion.x = vector.x;
            quaternion.y = vector.y;
            quaternion.z = vector.z;
            quaternion.w = Mathf.Sqrt(1.0f - sqrMagnitude) * Mathf.Sign(quaternion.w);

            return quaternion;
        }
    }
}