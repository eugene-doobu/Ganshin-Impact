namespace GanShin
{
    public class Define
    {
        public enum eLayer
        {
            CHARACTER = 3,
            GROUND    = 7,
            MONSTER   = 8,
            ENVIRONMENT = 9,
        }

        public enum ePlayerAvatar
        {
            RIKO,
            AI,
            MUSCLE_CAT,
            NONE
        }

        public enum eScene
        {
            UNKNOWN,
            LOADING_SCENE,
            INTRO,
            DEMO,
            SIMPLE_DEMO,
            VILLAGE,
            GAME
        }

        public enum eSound
        {
            BGM,
            EFFECT,
            MAX_COUNT,
        }

        public enum eUIEvent
        {
            CLICK,
            DRAG,
        }

        public static int GetLayerMask(eLayer layer)
        {
            return 1 << (int)layer;
        }

        public struct Tag
        {
            public static string Player       = "Player";
            public static string PlayerWeapon = "PlayerWeapon";
            public static string Monster      = "Monster";
        }
    }
}