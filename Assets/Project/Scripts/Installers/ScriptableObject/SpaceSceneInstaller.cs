using GanShin.Space.Content;
using UnityEngine;
using Zenject;

namespace GanShin
{
    [CreateAssetMenu(menuName = "Installers/SpaceSceneInstaller")]
    public class SpaceSceneInstaller : ScriptableObjectInstaller<SpaceSceneInstaller>
    {
        private const string UIPrefabName            = "Prefabs/UI/Root/Canvas_SpaceScene";
        private const string MinimapCameraPrefabName = "Prefabs/Camera/MinimapCamera";

        public override void InstallBindings()
        {
            Container.Bind<Camera>()
                .WithId(MinimapManager.MinimapCameraId)
                .FromComponentInNewPrefabResource(MinimapCameraPrefabName)
                .AsSingle()
                .NonLazy();

            Container.Bind<Canvas>()
                .FromComponentInNewPrefabResource(UIPrefabName)
                .AsSingle()
                .NonLazy();

            Container.Bind(
                    typeof(MinimapManager),
                    typeof(IInitializable),
                    typeof(ILateTickable)
                )
                .To<MinimapManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}
