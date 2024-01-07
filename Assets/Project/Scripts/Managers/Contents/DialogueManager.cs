#nullable enable

using GanShin.Dialogue.Base;
using GanShin.Space.UI;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class DialogueManager : ManagerBase
    {
        public DialogueContext Context { get; private set; } = new();

        private UIDialogue? _currentUI;
        
        private bool _isEnable;
        
        public override void Initialize()
        {
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                OnSpacePressed();

            if (Input.GetKeyDown(KeyCode.Escape))
                EndDialogue();
        }
        
        public void SetUI(UIDialogue ui)
        {
            _currentUI = ui;
        }
        
        public void DestroyUI(UIDialogue ui)
        {
            if (_currentUI != ui) return;
            _currentUI = null;
            _isEnable  = false;
        }
        
        public void StartDialogue()
        {
            if (!HasUIObject()) return;
            _currentUI!.Enable();
            _isEnable = true;
        }
        
        private void EndDialogue()
        {
            if (!_isEnable) return;
            if (!HasUIObject()) return;
            _currentUI!.Disable();
            _isEnable = false;
        }
        
        private void OnSpacePressed()
        {
            if (!_isEnable) return;
            if (!HasUIObject()) return;

            if (_currentUI!.IsOnTyping)
                _currentUI!.SkipDialogue();
            // else: 다음 대화로 넘어가기
        }

        public void SetString(DialogueInfo info)
        {
            if (!HasUIObject()) return;
            if (!_isEnable) StartDialogue();
            _currentUI!.SetDialogue(info);
        }

        private bool HasUIObject()
        {
            if (_currentUI != null) return true;
            GanDebugger.LogWarning(nameof(DialogueManager), "Current UI is null");
            return false;
        }
    }
}
