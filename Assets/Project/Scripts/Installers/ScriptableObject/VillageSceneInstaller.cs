using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GanShin.Village.Base
{
    [CreateAssetMenu(menuName = "Installers/VillageSceneInstaller")]
    public class VillageSceneInstaller : ScriptableObjectInstaller<VillageSceneInstaller>
    {
        public const string NpcInfoListId = "LoadingSceneInstaller.NpcInfoListId";
        
        /// <summary>
        /// 인스펙터에서의 제어를 위해 딕셔너리가 아닌 리스트로 구현
        /// </summary>
        public List<NpcInfo> npcInfoList = new();

        public override void InstallBindings()
        {
            Container.BindInstance(npcInfoList).WithId(NpcInfoListId);
        }
    }
}
