#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Space.Content;
using GanShin.UI;
using UnityEngine;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Space.UI
{
    public class UIDialogue : UIRootBase
    {
        [Inject] private DialogueManager? _manager;
        
        [SerializeField] private float delayTime = 0.1f;

        private string _dialogueString    = string.Empty;
        private string _currentViewString = string.Empty;
        
        private CancellationTokenSource? _dialogueMessageCts;
        
        private DialogueContext? _dialogueContext;
        
        private string CurrentViewString
        {
            get => _currentViewString;
            set
            {
                _currentViewString = value;
                _dialogueContext!.Content = _currentViewString;
            }
        }
        
        public bool IsOnTyping => _dialogueMessageCts != null;

#region Initialize
        protected override Context? InitializeDataContext()
        {
            if (_manager == null)
            {
                GanDebugger.LogError(GetType().Name, "DialogueManager is null");
                return null;
            }
            
            _manager.SetUI(this);
            _dialogueContext = _manager.Context;
            return _dialogueContext;
        }

        private void OnDestroy()
        {
            _manager?.DestroyUI(this);
        }

        public void Enable()
        {
            if (CanvasRoot != null)
                CanvasRoot.ActiveAllUIRoots(false, typeof(UIDialogue));

            _dialogueString = string.Empty;
            _currentViewString = string.Empty;
            Show();
        }

        public void Disable()
        {
            if (CanvasRoot != null)
                CanvasRoot.ActiveAllUIRoots(true, typeof(UIDialogue));

            Hide();
            SkipDialogue();
        }
#endregion Initialize

#region ContextControll
        public void SetString(string dialogueString)
        {
            SkipDialogue();
            _dialogueMessageCts = new CancellationTokenSource();
            SetStringAsync(dialogueString, _dialogueMessageCts).Forget();
        }
        
        private async UniTask SetStringAsync(string dialogueString, CancellationTokenSource cts)
        {
            CurrentViewString = string.Empty;
            _dialogueString   = dialogueString;
            
            while (CurrentViewString.Length < _dialogueString.Length)
            {
                var currentViewNum = Mathf.Max(1, Time.deltaTime / delayTime);

                for (var i = 0; i < currentViewNum; i++)
                {
                    if (CurrentViewString.Length >= _dialogueString.Length) break;
                    CurrentViewString         += _dialogueString[CurrentViewString.Length];
                }
                
                var isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: cts.Token).SuppressCancellationThrow();
                if (!isCancelled) continue;

                CurrentViewString = _dialogueString;
                return;
            }
        }
        
        public void SkipDialogue()
        {
            _dialogueMessageCts?.Cancel();
            _dialogueMessageCts?.Dispose();
            _dialogueMessageCts = null;
        }
#endregion ContextControll
    }
}
