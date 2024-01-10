namespace GanShin.Content
{
    public struct AvatarDefine
    {
        public static string WristRightBone = "Wrist_R";
        public static string WristLeftBone  = "Wrist_L";
    }

    public enum ePlayerAttack
    {
        NONE,
        RIKO_BASIC_ATTACK1,
        RIKO_BASIC_ATTACK2,
        RIKO_BASIC_ATTACK3,
        RIKO_BASIC_ATTACK4,
        RIKO_ULTIMATE_ATTACK1,
        RIKO_ULTIMATE_ATTACK2,
        RIKO_ULTIMATE_ATTACK3,
        RIKO_ULTIMATE_ATTACK4,
        MUSCLE_CAT_ATTACK1,
        MUSCLE_CAT_ATTACK2,
        MUSCLE_CAT_ATTACK3,
        MUSCLE_CAT_ATTACK4
    }

    public enum eMonsterState
    {
        CREATED,
        IDLE,
        TRACING,
        ATTACK,
        DEAD
    }
}