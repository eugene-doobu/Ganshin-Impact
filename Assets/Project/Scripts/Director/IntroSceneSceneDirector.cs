using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

#nullable enable

namespace GanShin.Director.IntroScene
{
    public class IntroSceneSceneDirector : TimelineDirector
    {
        [Inject] IntroSceneSettingInstaller.BridgeSettings _bridgeSettings = null!;

        [Inject] IntroSceneSettingInstaller.BridgePrefabs _bridgePrefabs = null!;

#region MonoBehaviour

        private void Start()
        {
            for (var i = 0; i < _bridgeSettings.numberOfBridge; ++i)
            {
                // TODO: Factory를 통해 생성하도록 수정
                var rand = Random.Range(0, 1f);
                var obj = Instantiate(rand >= _bridgeSettings.bridge1SpawnRate
                    ? _bridgePrefabs.bridge2
                    : _bridgePrefabs.bridge1);
                var yPos = i < _bridgeSettings.numOfY0BridgeWhenStart ? 0 : _bridgeSettings.bridgeSpawnYPosition;
                obj.transform.position = new Vector3(0, yPos, i * _bridgeSettings.bridgeDistanceInterval);

                rand = Random.Range(0, 1f);
                if (rand <= _bridgeSettings.pillarSpawnRate)
                {
                    rand = Random.Range(0, 1f);
                    var pillar = Instantiate(
                        rand >= _bridgeSettings.pillar1SpawnRate + _bridgeSettings.pillar2SpawnRate
                            ?
                            _bridgePrefabs.pillar3
                            : rand >= _bridgeSettings.pillar1SpawnRate
                                ? _bridgePrefabs.pillar2
                                : _bridgePrefabs.pillar1,
                        obj.transform, true);
                    pillar.transform.localPosition = Vector3.zero;
                }
            }
        }

#endregion MonoBehaviour
    }
}