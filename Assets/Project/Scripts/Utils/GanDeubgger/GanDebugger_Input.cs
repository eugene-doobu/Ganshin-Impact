namespace GanShin
{
    // GanDebugger_Input
    public static partial class GanDebugger
    {
        private static readonly string HEADER_INPUT = "InputSystem";

        public static bool IsDebuggingInput = false;

        public static void InputLog(string message)
        {
            if (!IsDebuggingInput) return;
            Log(HEADER_INPUT, message);
        }

        public static void InputLogWarning(string message)
        {
            if (!IsDebuggingInput) return;
            LogWarning(HEADER_INPUT, message);
        }

        public static void InputLogError(string message)
        {
            if (!IsDebuggingInput) return;
            LogError(HEADER_INPUT, message);
        }
    }
}