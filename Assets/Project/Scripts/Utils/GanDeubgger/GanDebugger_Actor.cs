namespace GanShin
{
    // GanDebugger_Actor
    public static partial class GanDebugger
    {
        private const string HEADER_ACTOR = "Actor";

        public static readonly bool IsDebuggingActor = true;

        public static void ActorLog(string message)
        {
            if (!IsDebuggingActor) return;
            Log(HEADER_ACTOR, message);
        }

        public static void ActorLogWarning(string message)
        {
            if (!IsDebuggingActor) return;
            LogWarning(HEADER_ACTOR, message);
        }

        public static void ActorLogError(string message)
        {
            if (!IsDebuggingActor) return;
            LogError(HEADER_ACTOR, message);
        }
    }
}