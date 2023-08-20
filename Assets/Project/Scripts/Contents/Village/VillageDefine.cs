using System;
using System.Collections.Generic;
using GanShin.Dialogue.Base;

namespace GanShin.Village.Base
{
    public enum ENpcType
    {
        NONE,
        SHOP,
    }

    public enum ENpcMenuType
    {
        TALK,
        SHOP,
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
