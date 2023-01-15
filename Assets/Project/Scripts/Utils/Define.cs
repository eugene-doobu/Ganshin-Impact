namespace GanShin
{
    public class Define
    {
        public enum eLayer
        {
            
        }
        
        public struct Tag
        {
            public static string Player  = "Player";
            public static string Monster = "Monster";
        }
        
        public enum eScene
        {
            Unknown,
            Login,
            Lobby,
            Game,
        }

        public enum eSound
        {
            Bgm,
            Effect,
            MaxCount,
        }

        public enum eUIEvent
        {
            Click,
            Drag,
        }
    }
}