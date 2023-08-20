using UnityEngine;
using Zenject;

namespace GanShin.Village.Base
{
    [CreateAssetMenu(menuName = "Installers/VillageSceneInstaller")]
    public class VillageSceneInstaller : ScriptableObjectInstaller<VillageSceneInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}
