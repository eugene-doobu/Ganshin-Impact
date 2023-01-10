using GanShin.AssetManagement;
using GanShin.CameraSystem;
using GanShin.InputSystem;
using GanShin.MapObject;
using GanShin.SceneManagement;
using GanShin.Sound;
using GanShin.UI;
using Zenject;

namespace Ganshin
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DataManager>().AsSingle().NonLazy();
            Container.Bind<PoolManager>().AsSingle().NonLazy();
            Container.Bind<ResourceManager>().AsSingle().NonLazy();
            Container.Bind<SceneManagerEx>().AsSingle().NonLazy();
            Container.Bind<SoundManager>().AsSingle().NonLazy();
            Container.Bind<UIManager>().AsSingle().NonLazy();
            Container.Bind<InputSystemManager>().AsSingle().NonLazy();
            Container.Bind<CameraManager>().AsSingle().NonLazy();
            Container.Bind<MapObjectManager>().AsSingle().NonLazy();
        }
    }
}