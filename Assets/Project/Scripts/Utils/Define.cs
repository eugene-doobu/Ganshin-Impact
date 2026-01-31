#nullable enable

namespace GanShin
{
    #region Layer & Tag

    public enum eLayer
    {
        CHARACTER   = 3,
        GROUND      = 7,
        MONSTER     = 8,
        ENVIRONMENT = 9,
    }

    /// <summary>
    /// eLayer 확장 메서드
    /// </summary>
    public static class LayerExtensions
    {
        public static int GetLayerMask(this eLayer layer)
        {
            return 1 << (int)layer;
        }
    }

    /// <summary>
    /// 태그 상수 정의
    /// </summary>
    public static class Tag
    {
        public const string Player       = "Player";
        public const string PlayerWeapon = "PlayerWeapon";
        public const string Monster      = "Monster";
    }

    #endregion Layer & Tag

    #region Scene & Sound

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

    #endregion Scene & Sound

    #region Player

    public enum ePlayerAvatar
    {
        RIKO,
        AI,
        MUSCLE_CAT,
        NONE
    }

    #endregion Player

    #region Event System

    /// <summary>
    /// EventManager에서 사용할 이벤트 타입
    /// </summary>
    public enum eEventType
    {
        // Player Events
        PLAYER_CHANGED,         // 플레이어 캐릭터 변경
        PLAYER_DAMAGED,         // 플레이어 피격
        PLAYER_HEALED,          // 플레이어 회복
        PLAYER_DIED,            // 플레이어 사망
        PLAYER_RESPAWNED,       // 플레이어 부활

        // Monster Events
        MONSTER_SPAWNED,        // 몬스터 스폰
        MONSTER_DAMAGED,        // 몬스터 피격
        MONSTER_KILLED,         // 몬스터 처치

        // Scene Events
        SCENE_LOADED,           // 씬 로드 완료
        SCENE_UNLOADED,         // 씬 언로드

        // Game State Events
        GAME_STARTED,           // 게임 시작
        GAME_PAUSED,            // 게임 일시정지
        GAME_RESUMED,           // 게임 재개
        GAME_OVER,              // 게임 오버

        // UI Events
        UI_OPENED,              // UI 열림
        UI_CLOSED,              // UI 닫힘
    }

    #endregion Event System

    #region UI System

    public enum eUIEvent
    {
        CLICK,
        DRAG,
    }

    /// <summary>
    /// UI 레이어 계층
    /// </summary>
    public enum eUILayer
    {
        SCENE,          // 게임 오브젝트 관련 UI (데미지 텍스트, 체력바)
        HUD,            // 화면 고정 UI (스킬 쿨다운, HP바)
        POPUP,          // 인게임 팝업 (인벤토리, 메뉴)
        NOTIFICATION,   // 알림 UI (토스트, 알림)
        GLOBAL,         // 글로벌 시스템 UI (로딩, 페이드)
    }

    /// <summary>
    /// UI Sort Order 상수
    /// </summary>
    public static class UISortOrder
    {
        public const int SCENE_UI     = 200;    // Layer 1: 게임 오브젝트 UI
        public const int HUD          = 300;    // Layer 2: 화면 고정 UI
        public const int POPUP        = 400;    // Layer 3: 인게임 팝업
        public const int NOTIFICATION = 500;    // Layer 4: 알림 UI
        public const int GLOBAL_UI    = 1000;   // Global: Dimmed, Popup, Toast, Loading

        public static int GetSortOrder(eUILayer layer) => layer switch
        {
            eUILayer.SCENE        => SCENE_UI,
            eUILayer.HUD          => HUD,
            eUILayer.POPUP        => POPUP,
            eUILayer.NOTIFICATION => NOTIFICATION,
            eUILayer.GLOBAL       => GLOBAL_UI,
            _                     => SCENE_UI
        };
    }

    #endregion UI System

    #region Creature State

    /// <summary>
    /// 크리처(캐릭터/몬스터) 공통 상태
    /// </summary>
    public enum eCreatureState
    {
        IDLE,
        MOVING,
        ATTACK,
        DAMAGED,
        DEAD,
    }

    #endregion Creature State
}