using UnityEngine;
using Zenject;

namespace GanShin.Director.IntroScene
{
    [CreateAssetMenu(menuName = "Installers/IntroSceneSettingInstaller")]
    public class IntroSceneSettingInstaller : ScriptableObjectInstaller<IntroSceneSettingInstaller>
    {
        private const string UIPrefabName = "Prefabs/UI/Root/Canvas_IntroScene";
        
        public override void InstallBindings()
        {
            Container.Bind<Canvas>()
                .FromComponentInNewPrefabResource(UIPrefabName)
                .AsSingle()
                .NonLazy();
        }
    }
}