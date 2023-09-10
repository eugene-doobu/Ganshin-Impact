using System;
using UnityEngine;

namespace GanShin.Dialogue.Base
{
    public enum ENpcDialogueImage
    {
        NONE,
        RIKO,
        AI,
        MUSCLE_CAT,
    }
    
    [Serializable]
    public class DialogueInfo
    {
        public string name;
        public string content;

        public ENpcDialogueImage npcDialogueImage;
    }
    
    [Serializable]
    public class DialogueImageInfo
    {        
        public ENpcDialogueImage type;
        public Sprite            sprite;
    }
}
