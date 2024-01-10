using DG.Tweening;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI
{
    public class UIRootCharacterCutScene : GlobalUIRootBase
    {
        private const float InitialPosition = -800;

        [SerializeField] private float onDuration  = 0.5f;
        [SerializeField] private float offDuration = 0.2f;
        [SerializeField] private float onDelay     = 0.7f;

        [SerializeField] private Ease onEase  = Ease.OutBack;
        [SerializeField] private Ease offEase = Ease.InBack;

        [SerializeField] private RectTransform rikoRoot;
        [SerializeField] private RectTransform aiRoot;
        [SerializeField] private RectTransform muscleCatRoot;

        public void OnCharacterCutScene(Define.ePlayerAvatar avatar)
        {
            RectTransform target = null;
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    target = rikoRoot;
                    break;
                case Define.ePlayerAvatar.AI:
                    target = aiRoot;
                    break;
                case Define.ePlayerAvatar.MUSCLE_CAT:
                    target = muscleCatRoot;
                    break;
            }

            if (target == null) return;

            target.DOAnchorPosX(0, onDuration).SetEase(onEase).OnComplete(() =>
            {
                target.DOAnchorPosX(
                        InitialPosition, offDuration)
                    .SetEase(offEase).SetDelay(onDelay);
            });
        }

#region GlobalUIRootBase

        protected override Context InitializeDataContext()
        {
            return null;
        }

        public override void InitializeContextData()
        {
        }

        public override void ClearContextData()
        {
        }

#endregion GlobalUIRootBase
    }
}