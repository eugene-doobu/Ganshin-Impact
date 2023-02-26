using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.Director.IntroScene
{
    [CreateAssetMenu(menuName = "Installers/IntroSceneSettingInstaller")]
    public class IntroSceneSettingInstaller : ScriptableObjectInstaller<IntroSceneSettingInstaller>
    {
        [Serializable]
        [UsedImplicitly]
        public class BridgeSettings
        {
            public int   numberOfBridge         = 30;
            public int   numOfY0BridgeWhenStart = 2;
            public float bridgeDistanceInterval = 5f;
            public float bridgeUpSpeed          = 1f;
            public float bridgeSpawnYPosition   = -2.5f;

            [Tooltip("bridge2의 스폰 확률은 1-'bridge1SpawnRate'")]
            public float bridge1SpawnRate = 0.8f;

            [Tooltip("bridge생성시 pillar가 생성될 확률")] public float pillarSpawnRate = 0.3f;

            [Tooltip("pillar3의 스폰 확률은 1-'pillar1SpawnRate'-'pillar2SpawnRate'")]
            public float pillar1SpawnRate = 0.3f;

            public float pillar2SpawnRate = 0.3f;
        }

        [Serializable]
        [UsedImplicitly]
        public class BridgePrefabs
        {
            public GameObject bridge1;
            public GameObject bridge2;
            public GameObject pillar1;
            public GameObject pillar2;
            public GameObject pillar3;
        }

        public BridgeSettings bridgeSettings;
        public BridgePrefabs  bridgePrefabs;
        public string         directorPrefabPath = "Prefabs/Decoration/IntroScene/IntroSceneDirector";

        public override void InstallBindings()
        {
            Container.BindInstance(bridgeSettings)
                .AsSingle()
                .NonLazy();
            ;
            Container.BindInstance(bridgePrefabs)
                .AsSingle()
                .NonLazy();
            ;
            Container.Bind<IntroSceneSceneDirector>()
                .FromComponentInNewPrefabResource(directorPrefabPath)
                .AsSingle()
                .NonLazy();
        }
    }
}