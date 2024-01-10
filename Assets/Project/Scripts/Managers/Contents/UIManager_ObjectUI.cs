#nullable enable

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GanShin.CameraSystem;
using GanShin.GanObject;
using GanShin.Space.UI;
using UnityEngine;

namespace GanShin.UI
{
    public partial class UIManager
    {
        private const    int        HUDCheckInterval = 100;
        private readonly List<long> _nearByObjectIds = new();

        private readonly Dictionary<long, Actor> _nearByObjects = new();
        
        private CancellationTokenSource? _cancellationToken;

        private FieldMonsterManagerContext? _fieldMonsterManagerContext;

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
                if (_fieldMonsterManagerContext == null ||
                    !_fieldMonsterManagerContext.TryGet(currId, out var currContext) ||
                    currContext == null)
                    continue;
                currContext.SortOrder = i;
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

            var actorManagerContext = GetOrAddContext<FieldMonsterManagerContext>();
            if (actorManagerContext != null)
            {
                _fieldMonsterManagerContext = actorManagerContext;
                actorManagerContext.Enable  = true;
            }
        }

        public void DisableControlObjectUI()
        {
            if (_fieldMonsterManagerContext != null)
            {
                _fieldMonsterManagerContext.Dispose();
                _fieldMonsterManagerContext = null;
            }
            
            _nearByObjects.Clear();
            _nearByObjectIds.Clear();

            if (_cancellationToken == null) return;
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = null;
        }
    }
}