using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin
{
    // GanDebugger_Input
    public static partial class GanDebugger
    {
        private static string HEADER_INPUT_MANAGER = "InputSystemManager";
        
        public static bool IsDebuggingInput = true;
        
        public static void InputLog(string message)
        {
            if (!IsDebuggingInput) return;
            Log(HEADER_INPUT_MANAGER, message);
        }
        
        public static void InputLogWarning(string message)
        {
            if (!IsDebuggingInput) return;
            LogWarning(HEADER_INPUT_MANAGER, message);
        }
        
        public static void InputLogError(string message)
        {
            if (!IsDebuggingInput) return;
            LogError(HEADER_INPUT_MANAGER, message);
        }
    }
}
