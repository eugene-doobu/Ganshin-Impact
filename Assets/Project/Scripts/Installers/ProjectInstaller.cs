using System;
using GanShin.CameraSystem;
using GanShin.InputSystem;
using GanShin.MapObject;
using GanShin.SceneManagement;
using GanShin.Sound;
using GanShin.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Ganshin
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallManagers();
            InstallCameras();
            InstallEssentialObjects();
            InstallGlobalUIs();
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

        private void InstallGlobalUIs()
        {
            Container.Bind<UIRootLoadingScene>()
                .FromComponentInNewPrefabResource(UIManager.GlobalUIName.Loading)
                .AsSingle()
                .NonLazy();
        }
    }
}