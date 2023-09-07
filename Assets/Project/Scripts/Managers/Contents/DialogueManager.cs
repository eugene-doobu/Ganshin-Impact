#nullable enable

using GanShin.Space.UI;
using JetBrains.Annotations;
using Zenject;

namespace GanShin.Space.Content
{
    [UsedImplicitly]
    public class DialogueManager : IInitializable
    {
        public DialogueContext Context { get; private set; } = new();

        private UIDialogue? _currentUI;
        
        public void Initialize()
        {
            _currentUI = null;
        }
        
        public void SetUI(UIDialogue ui)
        {
            _currentUI = ui;
        }
        
        public void DestroyUI(UIDialogue ui)
        {
            if (_currentUI == ui)
                _currentUI = null;
        }
        
        public void StartDialogue()
        {
            if (!HasUIObject()) return;
            _currentUI!.Enable();
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
