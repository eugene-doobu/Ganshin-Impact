#nullable enable

using GanShin.Space.UI;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class DialogueManager : IInitializable, ITickable
    {
        public DialogueContext Context { get; private set; } = new();

        private UIDialogue? _currentUI;
        
        private bool _isEnable;
        
        public void Initialize()
        {
        }

        public void Tick()
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

        // TODO: DialogueInfo 버전 만들기
        public void SetString(string dialogueString)
        {
            if (!HasUIObject()) return;
            _currentUI!.SetString(dialogueString);
        }

        private bool HasUIObject()
        {
            if (_currentUI != null) return true;
            GanDebugger.LogWarning(nameof(DialogueManager), "Current UI is null");
            return false;
        }
    }
}
