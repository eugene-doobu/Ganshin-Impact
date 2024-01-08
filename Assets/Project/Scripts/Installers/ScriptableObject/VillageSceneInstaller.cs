using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Village.Base
{
    public class VillageSceneInstaller : ScriptableObject
    {
        public const string NpcInfoListId = "LoadingSceneInstaller.NpcInfoListId";
        
        /// <summary>
        /// 인스펙터에서의 제어를 위해 딕셔너리가 아닌 리스트로 구현
        /// </summary>
        public List<NpcInfo> npcInfoList = new();
    }
}
