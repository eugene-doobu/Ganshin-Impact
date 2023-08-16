using GanShin;
using GanShin.CameraSystem;
using GanShin.Content.Creature;
using GanShin.Effect;
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
                    typeof(IInitializable),
                    typeof(ITickable)
                )
                .To<PlayerManager>()
                .AsSingle().NonLazy();
            Container.Bind<EffectManager>().AsSingle().NonLazy();
        }

        private void InstallCameras()
        {
            Container.Bind<CharacterCamera>().AsSingle().NonLazy();
            Container.Bind<CharacterUltimateCamera>().AsSingle().NonLazy();
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
            Container.Bind<RikoController>()
                .WithId(PlayerManager.AvatarBindId.Riko)
                .FromComponentInNewPrefabResource(PlayerManager.AvatarPath.Riko)
                .AsSingle()
                .NonLazy();
            Container.Bind<AiController>()
                .WithId(PlayerManager.AvatarBindId.Ai)
                .FromComponentInNewPrefabResource(PlayerManager.AvatarPath.Ai)
                .AsSingle()
                .NonLazy();
            Container.Bind<MuscleCatController>()
                .WithId(PlayerManager.AvatarBindId.MuscleCat)
                .FromComponentInNewPrefabResource(PlayerManager.AvatarPath.MuscleCat)
                .AsSingle()
                .NonLazy();
        }

        private void InstallGlobalUIs()
        {
            Container.Bind<UIRootLoadingScene>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.LoadingScene)
                .AsSingle()
                .NonLazy();
            Container.Bind<UIRootCharacterCutScene>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.CharacterCutScene)
                .AsSingle()
                .NonLazy();
            Container.Bind<UIRootDimmed>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.Dimmed)
                .AsSingle()
                .NonLazy();
            Container.Bind<UIRootPopup>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.Popup)
                .AsSingle()
                .NonLazy();
            Container.Bind<UIRootToastPopup>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.Toast)
                .AsSingle()
                .NonLazy();
            Container.Bind<UIRootLoadingPopup>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.Loading)
                .AsSingle()
                .NonLazy();
        }
    }
}