using UnityEngine;
using Zenject;

namespace GanShin
{
    [CreateAssetMenu(menuName = "Installers/SpaceSceneInstaller")]
    public class SpaceSceneInstaller : ScriptableObjectInstaller<SpaceSceneInstaller>
    {
        private const string UIPrefabName = "Prefabs/UI/Root/Canvas_SpaceScene";

        public override void InstallBindings()
        {
            Container.Bind<Canvas>()
                .FromComponentInNewPrefabResource(UIPrefabName)
                .AsSingle()
                .NonLazy();
        }
    }
}
