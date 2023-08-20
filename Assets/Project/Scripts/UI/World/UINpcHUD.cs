using System.Collections.Generic;
using GanShin.UI;
using GanShin.Village.Base;
using UnityEngine;
using Zenject;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Village.UI
{
    public class UINpcHUD : UIRootBase
    {
        [SerializeField] private ENpcType npcType = ENpcType.NONE;

        [Inject(Id = VillageSceneInstaller.NpcInfoId)]
        private List<NpcInfo> _npcInfoDic;

        protected override Context InitializeDataContext()
        {
            foreach (var npcInfo in _npcInfoDic)
                if (npcInfo.npcType == npcType)
                    return new NpcContext(this, npcInfo);

            return new NpcContext(this);
        }
    }
}
