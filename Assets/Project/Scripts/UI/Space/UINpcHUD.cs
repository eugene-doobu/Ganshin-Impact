using System.Collections.Generic;
using GanShin.UI;
using GanShin.Village.Base;
using GanShin.Village.Contents;
using UnityEngine;
using Context = Slash.Unity.DataBind.Core.Data.Context;

namespace GanShin.Village.UI
{
    public class UINpcHUD : UIRootBase
    {
        [SerializeField] private NpcObject owner;

        // TODO: Addressable로 변경
        //[Inject(Id = VillageSceneInstaller.NpcInfoListId)]
        private List<NpcInfo> _npcInfoDic;

        protected override Context InitializeDataContext()
        {
            foreach (var npcInfo in _npcInfoDic)
                if (npcInfo.npcType == owner.NpcType)
                    return new NpcContext(this, npcInfo);

            return new NpcContext(this);
        }
    }
}
