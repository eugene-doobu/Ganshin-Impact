using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin
{
    // GanDebugger_Camera
    public static partial class GanDebugger
    {
        private static string HEADER_CAMERA = "CameraSystem";

        public static bool IsDebuggingCamera = true;

        public static void CameraLog(string message)
        {
            if (!IsDebuggingCamera) return;
            Log(HEADER_CAMERA, message);
        }

        public static void CameraLogWarning(string message)
        {
            if (!IsDebuggingCamera) return;
            LogWarning(HEADER_CAMERA, message);
        }

        public static void CameraLogError(string message)
        {
            if (!IsDebuggingCamera) return;
            LogError(HEADER_CAMERA, message);
        }
    }
}