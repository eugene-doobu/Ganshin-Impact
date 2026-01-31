using System;
using System.Collections.Generic;
using GanShin.GameContent.Dialogue;

namespace GanShin.GameContent.Village
{
    public enum ENpcType
    {
        NONE,
        CHUN_BAE,
        YEONG_CHUL
    }

    [Flags]
    public enum ENpcMenuType
    {
        TALK,
        SHOP
    }

    [Serializable]
    public class NpcInfo
    {
        public ENpcType           npcType;
        public string             npcName;
        public string             npcTitle;
        public ENpcMenuType       npcMenuType;
        public List<DialogueInfo> npcDialogueList;
    }

    public static class VillageDefine
    {
    }
}