using System.Collections.Generic;
using GanShin.Dialogue.Base;
using UnityEngine;

namespace GanShin
{
    public class SpaceSceneInstaller : ScriptableObject
    {
        public const string DialogueImageInfoId = "SpaceSceneInstaller.DialogueImageInfoId";

        [SerializeField] private DialogueImageInfo[] dialogueImageInfos;
        
        private readonly Dictionary<ENpcDialogueImage, Sprite> _dialogueImageInfoDic = new();

        private void InitializeDialogueImageDict()
        {
            _dialogueImageInfoDic.Clear();
            foreach (var info in dialogueImageInfos)
                _dialogueImageInfoDic.Add(info.type, info.sprite);
        }
    }
}
