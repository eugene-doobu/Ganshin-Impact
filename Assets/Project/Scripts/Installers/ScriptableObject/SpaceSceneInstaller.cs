using System.Collections.Generic;
using GanShin.Dialogue.Base;
using UnityEngine;

namespace GanShin
{
    public class SpaceSceneInstaller : ScriptableObject
    {
        [SerializeField] private DialogueImageInfo[] dialogueImageInfos;
        
        public Dictionary<ENpcDialogueImage, Sprite> DialogueImageInfoDic { get; } = new();

        private void Awake()
        {
            InitializeDialogueImageDict();
        }

        private void InitializeDialogueImageDict()
        {
            DialogueImageInfoDic.Clear();
            foreach (var info in dialogueImageInfos)
                DialogueImageInfoDic.Add(info.type, info.sprite);
        }
    }
}
