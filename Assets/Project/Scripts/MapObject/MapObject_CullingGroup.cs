using System;
using GanShin.CameraSystem;
using UnityEngine;

namespace GanShin.GanObject
{
    public enum eHudCullingState
    {
        ENABLE,
        DISABLE
    }

    public abstract partial class MapObject : ICullingTarget
    {
        [SerializeField] private eCullingUpdateMode _boundingSphereUpdateMode = eCullingUpdateMode.DYNAMIC;
        [SerializeField] private float              _cullingBoundingRadius    = 0.33f;

        private BoundingSphere   _boundingSphere;
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

        public CullingGroup.StateChanged OnHudStateChanged { get; }

        public eCullingUpdateMode BoundingSphereUpdateMode => _boundingSphereUpdateMode;
        public BoundingSphere     BoundingSphere           => _boundingSphere;

        public BoundingSphere UpdateAndGetBoundingSphere()
        {
            _boundingSphere.position = transform.position + Vector3.up * ObjectHeight;
            _boundingSphere.radius   = _cullingBoundingRadius;
            return _boundingSphere;
        }

        public event Action<MapObject> OnBecomeOccluded;
        public event Action<MapObject> OnBecomeUnoccluded;

        public event Action<MapObject, eHudCullingState> OnHudCullingStateChanged;

        // 오브젝트 생성 / 삭제시 이벤트에 연동
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
                case eHudCullingState.ENABLE:
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
                case eHudCullingState.ENABLE:
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