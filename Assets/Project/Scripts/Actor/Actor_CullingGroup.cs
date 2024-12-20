using System;
using GanShin.CameraSystem;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.GanObject
{
    public enum eHudCullingState
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        DISABLE,
    }

    public abstract partial class Actor : ICullingTarget
    {
        [SerializeField] private eCullingUpdateMode _boundingSphereUpdateMode = eCullingUpdateMode.DYNAMIC;
        [SerializeField] private float              _cullingBoundingRadius    = 0.5f;

        private BoundingSphere   _boundingSphere;
        [SerializeField, ReadOnly]
        private eHudCullingState _hudCullingState = eHudCullingState.DISABLE;

        public bool IsOccluded => HudCullingState == eHudCullingState.DISABLE;

        public eHudCullingState HudCullingState
        {
            get => _hudCullingState;
            set
            {
                if (_hudCullingState != value)
                    OnHudCullingStateChanged?.Invoke(this, value);
                _hudCullingState = value;
            }
        }

        public CullingGroupProxy HudCullingGroup { get; set; }

        public CullingGroup.StateChanged OnHudStateChanged { get; set; }

        public eCullingUpdateMode BoundingSphereUpdateMode => _boundingSphereUpdateMode;
        public BoundingSphere     BoundingSphere           => _boundingSphere;

        public BoundingSphere UpdateAndGetBoundingSphere()
        {
            _boundingSphere.position = transform.position + Vector3.up * ObjectHeight;
            _boundingSphere.radius   = _cullingBoundingRadius;
            return _boundingSphere;
        }

        public event Action<Actor> OnBecomeOccluded;
        public event Action<Actor> OnBecomeUnoccluded;

        public event Action<Actor, eHudCullingState> OnHudCullingStateChanged;

        private void OnHudCullingGroupStateChanged(CullingGroupEvent cullingGroupEvent)
        {
            var prevState = HudCullingState;
            SetHudCullingState(cullingGroupEvent);

            if (prevState == HudCullingState)
                return;

            OnChangePrevState(prevState);
            OnChangeState(HudCullingState);
        }

        private void OnChangePrevState(eHudCullingState prevState)
        {
            switch (prevState)
            {
                case eHudCullingState.LEVEL_1:
                    break;
                case eHudCullingState.LEVEL_2:
                    break;
                case eHudCullingState.LEVEL_3:
                    break;
                case eHudCullingState.DISABLE:
                    OnBecomeUnoccluded?.Invoke(this);
                    break;
            }
        }

        private void OnChangeState(eHudCullingState state)
        {
            switch (state)
            {
                case eHudCullingState.LEVEL_1:
                    break;
                case eHudCullingState.LEVEL_2:
                    break;
                case eHudCullingState.LEVEL_3:
                    break;
                case eHudCullingState.DISABLE:
                    OnBecomeOccluded?.Invoke(this);
                    break;
            }
        }

        private void SetHudCullingState(CullingGroupEvent cullingGroupEvent)
        {
            if (!cullingGroupEvent.isVisible)
            {
                HudCullingState = eHudCullingState.DISABLE;
                return;
            }

            HudCullingState = (eHudCullingState)cullingGroupEvent.currentDistance;
        }
    }
}