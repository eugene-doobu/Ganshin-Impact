#nullable enable

using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GanShin.CameraSystem
{
    [RequireComponent(typeof(Camera))]
    public class GanCamera : MonoBehaviour
    {
        private readonly Dictionary<eCullingGroupType, CullingGroupProxy> _cullingGroupProxies = new();

        private float _defaultBlendTime;

        public Camera?           Camera { get; private set; }
        public CinemachineBrain? Brain  { get; private set; }

#region MonoBehaviour
        private void Awake()
        {
            Camera            = GetComponent<Camera>();
            Brain             = Util.GetOrAddComponent<CinemachineBrain>(gameObject);
            _defaultBlendTime = Brain.m_DefaultBlend.m_Time;

            gameObject.tag = "MainCamera";

            // TODO: CameraManager 주입 후 MainVirtualCamera 관리
        }
#endregion MonoBehaviour

        public CullingGroupProxy? GetOrAddCullingGroupProxy(eCullingGroupType cullingGroupType)
        {
            if (_cullingGroupProxies.TryGetValue(cullingGroupType, out var cullingGroupProxy) &&
                cullingGroupProxy != null)
                return cullingGroupProxy;

            var cameraManager = ProjectManager.Instance.GetManager<CameraManager>();
            if (cameraManager == null)
            {
                GanDebugger.CameraLogError("Failed to get camera manager");
                return null;
            }

            var newCullingGroupProxy = cameraManager.SetCullingGroupProxy(gameObject, cullingGroupType);
            _cullingGroupProxies[cullingGroupType] = newCullingGroupProxy;

            return newCullingGroupProxy;
        }

        public void RegisterOnBlendingCompleteAction(Action? onComplete)
        {
            if (onComplete == null) return;
            BlendingCompleteChecker(onComplete).Forget();
        }

        private async UniTask BlendingCompleteChecker(Action oncomplete)
        {
            await UniTask.NextFrame();
            await UniTask.WaitUntil(IsBlending);
            oncomplete.Invoke();
        }

        private bool IsBlending()
        {
            return Brain != null && Brain.IsBlending;
        }

        public void SetDefaultBlendTime()
        {
            if (Brain != null)
                Brain.m_DefaultBlend.m_Time = _defaultBlendTime;
        }
    }
}