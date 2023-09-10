using System;

namespace GanShin.Dialogue.Base
{
    /// <summary>
    /// 현재 대화중인 대상의 타입을 지정, 선택된 타입으로 대화중인 캐릭터 이미지가 활성화된다.
    /// </summary>
    public enum EDialogueDirection
    {
        NONE, // 이미지 없이 대사만 출력
        PLAYER, // 현재 선택된 플레이어의 대사
        NPC,
    }

    public enum ENpcDialogueImage
    {
        NPC,
    }
    
    [Serializable]
    public class DialogueInfo
    {
        public string content;

        public EDialogueDirection direction;
        public ENpcDialogueImage npcDialogueImage;
    }
}
