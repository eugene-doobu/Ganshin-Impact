using UnityEngine;

namespace GanShin
{
    public static partial class GanDebugger
    {
        public static bool IsDebugging = true;

        public static void Log(string message)
        {
            if (!IsDebugging) return;
            Debug.Log(message);
        }

        public static void Log(string header, string message)
        {
            if (!IsDebugging) return;
            Debug.Log($"[{header}]: {message}");
        }

        public static void Log(string header, string message, Object context)
        {
            if (!IsDebugging) return;
            Debug.Log($"[{header}]: {message}", context);
        }

        public static void LogWarning(string message)
        {
            if (!IsDebugging) return;
            Debug.LogWarning(message);
        }

        public static void LogWarning(string header, string message)
        {
            if (!IsDebugging) return;
            Debug.LogWarning($"[{header}]: {message}");
        }

        public static void LogWarning(string header, string message, Object context)
        {
            if (!IsDebugging) return;
            Debug.LogWarning($"[{header}]: {message}", context);
        }

        public static void LogError(string message)
        {
            if (!IsDebugging) return;
            Debug.LogError(message);
        }

        public static void LogError(string header, string message)
        {
            if (!IsDebugging) return;
            Debug.LogError($"[{header}]: {message}");
        }

        public static void LogError(string header, string message, Object context)
        {
            if (!IsDebugging) return;
            Debug.LogError($"[{header}]: {message}", context);
        }
    }
}