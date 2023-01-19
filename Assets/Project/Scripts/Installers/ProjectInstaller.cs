using System;
using GanShin.AssetManagement;
using GanShin.CameraSystem;
using GanShin.InputSystem;
using GanShin.MapObject;
using GanShin.SceneManagement;
using GanShin.Sound;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace Ganshin
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallManagers();
            InstallCameras();
        }
        
        private void InstallManagers()
        {
            Container.Bind<DataManager>().AsSingle().NonLazy();
            Container.Bind<PoolManager>().AsSingle().NonLazy();
            Container.Bind<ResourceManager>().AsSingle().NonLazy();
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
    }
}