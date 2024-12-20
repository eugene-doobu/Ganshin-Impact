#nullable enable

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.Dialogue.Base;
using GanShin.Space.Content;
using GanShin.UI;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI.Space
{
    public class UIDialogue : UIRootBase
    {
        [Header("UIDialogue")] [SerializeField]
        private float delayTime = 0.1f;

        private readonly DialogueManager? _manager = ProjectManager.Instance.GetManager<DialogueManager>();

        private string _currentViewString = string.Empty;

        private DialogueContext? _dialogueContext;

        private CancellationTokenSource? _dialogueMessageCts;

        private string _dialogueString = string.Empty;

        private string CurrentViewString
        {
            get => _currentViewString;
            set
            {
                _currentViewString        = value;
                _dialogueContext!.Content = _currentViewString;
            }
        }

        public bool IsOnTyping => _dialogueMessageCts != null;

        private Sprite? GetDialogueImage(ENpcDialogueImage type)
        {
            var spaceSceneData = Util.LoadAsset<SpaceSceneInstaller>("SpaceScene.asset");
            if (spaceSceneData == null)
            {
                GanDebugger.LogError(GetType().Name, "Failed to get space scene data");
                return null;
            }

            var npcDialogueImages = spaceSceneData.DialogueImageInfoDic;
            if (npcDialogueImages == null || !npcDialogueImages.ContainsKey(type))
                return null;

            return npcDialogueImages[type];
        }

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

            _dialogueString    = string.Empty;
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

        public void SetDialogue(DialogueInfo info)
        {
            if (_dialogueContext == null)
            {
                GanDebugger.LogError(GetType().Name, "DialogueContext is null");
                return;
            }

            SetString(info.content);
            _dialogueContext.Name   = info.name;
            _dialogueContext.Sprite = GetDialogueImage(info.npcDialogueImage);
        }

        private void SetString(string dialogueString)
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
                    CurrentViewString += _dialogueString[CurrentViewString.Length];
                }

                var isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: cts.Token)
                    .SuppressCancellationThrow();
                if (!isCancelled) continue;
                if (_dialogueMessageCts != null && cts != _dialogueMessageCts) return;

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