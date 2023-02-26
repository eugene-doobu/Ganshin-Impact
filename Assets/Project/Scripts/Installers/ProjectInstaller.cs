using GanShin;
using GanShin.CameraSystem;
using GanShin.Content.Creature;
using GanShin.InputSystem;
using GanShin.MapObject;
using GanShin.SceneManagement;
using GanShin.Sound;
using GanShin.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace Ganshin
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallManagers();
            InstallEssentialObjects();
            InstallCharacterObjects();
            InstallGlobalUIs();
            InstallCameras();
        }

        private void InstallManagers()
        {
            Container.Bind<InputSystemManager>().AsSingle().NonLazy();
            Container.Bind<SceneManagerEx>().AsSingle().NonLazy();
            Container.Bind<SoundManager>().AsSingle().NonLazy();
            Container.Bind(
                    typeof(UIManager),
                    typeof(IInitializable)
                )
                .To<UIManager>().AsSingle().NonLazy();
            Container.Bind(
                    typeof(CameraManager),
                    typeof(IInitializable),
                    typeof(ITickable),
                    typeof(ILateTickable)
                )
                .To<CameraManager>()
                .AsSingle()
                .NonLazy();
            Container.Bind<MapObjectManager>().AsSingle().NonLazy();
            Container.Bind(
                    typeof(PlayerManager),
                    typeof(IInitializable)
                )
                .To<PlayerManager>()
                .AsSingle().NonLazy();
        }

        private void InstallCameras()
        {
            Container.Bind<CharacterCamera>().AsSingle().NonLazy();
        }

        private void InstallEssentialObjects()
        {
            Container.Bind<EventSystem>()
                .FromComponentInNewPrefabResource(UIManager.EventSystemPath)
                .AsSingle()
                .NonLazy();
        }

        private void InstallCharacterObjects()
        {
            Container.Bind<PlayerController>()
                .WithId(PlayerManager.AvatarBindId.Riko)
                .FromComponentInNewPrefabResource(PlayerManager.AvatarPath.Riko)
                .AsSingle()
                .NonLazy();
        }

        private void InstallGlobalUIs()
        {
            Container.Bind<UIRootLoadingScene>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.Loading)
                .AsSingle()
                .NonLazy();
        }
    }
}