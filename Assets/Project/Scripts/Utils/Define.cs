namespace GanShin
{
    public class Define
    {
        public enum eLayer
        {
            CHARACTER = 3,
            MONSTER = 8
        }

        public static int GetLayerMask(eLayer layer)
        {
            return 1 << (int) layer;
        }

        public struct Tag
        {
            public static string Player       = "Player";
            public static string PlayerWeapon = "PlayerWeapon";
            public static string Monster      = "Monster";
        }

        public enum ePlayerAvatar
        {
            RIKO,
            AI,
            MUSCLE_CAT
        }

        public enum eScene
        {
            Unknown,
            LoadingScene,
            LobbyScene,
            Demo,
            SimpleDemo,
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