#nullable enable

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.GanObject;
using UnityEngine;

namespace GanShin.UI
{
    public partial class UIManager
    {
        private const    int        HUDCheckInterval = 100;
        private readonly List<long> _nearByObjectIds = new();

        private readonly Dictionary<long, Actor> _nearByObjects = new();
        
        private CancellationTokenSource? _cancellationToken;

        private async UniTask HudDistanceChecker(CancellationTokenSource cancellationToken)
        {
            while (await UniTaskHelper.Delay(HUDCheckInterval, cancellationToken))
                SetHudSortOrder();
        }

        public void AddNearByObject(Actor actor)
        {
            _nearByObjects.TryAdd(actor.Id, actor);
        }	
		
        public void RemoveNearByObject(Actor actor)
        {
            _nearByObjects.Remove(actor.Id);
        }

        private void SetHudSortOrder()
        {
            _nearByObjectIds.Clear();
            foreach (var key in _nearByObjects.Keys)
                _nearByObjectIds.Add(key);
            _nearByObjectIds.Sort(SortDistanceComparision);

            for (var i = 0; i < _nearByObjectIds.Count; ++i)
            {
                var currId = _nearByObjectIds[i];
                // GetContext, Set SortOrder
            }
        }

        private int SortDistanceComparision(long a, long b)
        {
            var mainCamera = ProjectManager.Instance.GetManager<CameraManager>()?.MainCamera;
            if (mainCamera == null) return 0;
            var cameraPosition = mainCamera.transform.position;

            if (!_nearByObjects.TryGetValue(a, out var objectA) || objectA == null)
                return -1;

            if (!_nearByObjects.TryGetValue(b, out var objectB) || objectB == null)
                return 1;

            var disA = Vector3.SqrMagnitude(objectA.transform.position - cameraPosition);
            var disB = Vector3.SqrMagnitude(objectB.transform.position - cameraPosition);
            return disA < disB ? 1 : -1;
        }

        public void EnableControlObjectUI()
        {
            if (_cancellationToken != null) return;
            _cancellationToken = new CancellationTokenSource();
            HudDistanceChecker(_cancellationToken).Forget();

            // ManagerContext 셋팅
        }

        public void DisableControlObjectUI()
        {
            // ManagerContext = null
            _nearByObjects.Clear();
            _nearByObjectIds.Clear();

            if (_cancellationToken == null) return;
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = null;
        }
    }
}