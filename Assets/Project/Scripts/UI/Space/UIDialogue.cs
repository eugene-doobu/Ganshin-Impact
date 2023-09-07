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
            _dialogueString = string.Empty;
            _currentViewString = string.Empty;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
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
            _currentViewString = string.Empty;
            _dialogueString = dialogueString;
            
            while (_currentViewString.Length < _dialogueString.Length)
            {
                var currentViewNum = Mathf.Max(1, Time.deltaTime / delayTime);

                for (var i = 0; i < currentViewNum; i++)
                {
                    if (_currentViewString.Length >= _dialogueString.Length) break;
                    _currentViewString += _dialogueString[i];
                }
                
                var isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: cts.Token).SuppressCancellationThrow();
                if (!isCancelled) continue;

                _currentViewString = _dialogueString;
                return;
            }
        }
        
        private void SkipDialogue()
        {
            _dialogueMessageCts?.Cancel();
            _dialogueMessageCts?.Dispose();
            _dialogueMessageCts = null;
        }
#endregion ContextControll
    }
}
