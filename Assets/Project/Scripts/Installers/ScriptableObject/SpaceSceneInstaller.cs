using System;
using System.Collections.Generic;
using GanShin.Dialogue.Base;
using UnityEngine;

namespace GanShin
{
    public class SpaceSceneInstaller : ScriptableObject
    {
        [SerializeField] private DialogueImageInfo[] dialogueImageInfos;
        
        private readonly Dictionary<ENpcDialogueImage, Sprite> _dialogueImageInfoDic = new();

        private void Awake()
        {
            InitializeDialogueImageDict();
        }

        private void InitializeDialogueImageDict()
        {
            _dialogueImageInfoDic.Clear();
            foreach (var info in dialogueImageInfos)
                _dialogueImageInfoDic.Add(info.type, info.sprite);
        }
    }
}
