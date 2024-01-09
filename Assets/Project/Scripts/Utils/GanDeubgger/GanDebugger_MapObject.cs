namespace GanShin
{
    // GanDebugger_MapObject
    public static partial class GanDebugger
    {
        private static readonly string HEADER_MAPOBJECT = "MapObject";

        public static bool IsDebuggingMapObject = true;

        public static void MapObjectLog(string message)
        {
            if (!IsDebuggingMapObject) return;
            Log(HEADER_MAPOBJECT, message);
        }

        public static void MapObjectLogWarning(string message)
        {
            if (!IsDebuggingMapObject) return;
            LogWarning(HEADER_MAPOBJECT, message);
        }

        public static void MapObjectLogError(string message)
        {
            if (!IsDebuggingMapObject) return;
            LogError(HEADER_MAPOBJECT, message);
        }
    }
}