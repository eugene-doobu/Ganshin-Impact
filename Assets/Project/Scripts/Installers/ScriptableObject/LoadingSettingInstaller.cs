using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GanShin
{
    [CreateAssetMenu(menuName = "Installers/LoadingSettingInstaller")]
    public class LoadingSettingInstaller : ScriptableObjectInstaller<LoadingSettingInstaller>
    {
        public const string TipsId                 = "LoadingSceneInstaller.tips";
        public const string ProgressSmoothFactorId = "LoadingSceneInstaller.progressSmoothFactor";
        public const string ChangeSceneDelayId     = "LoadingSceneInstaller.changeSceneDelay";

        public List<string> tips;
        public float        progressSmoothFactor;
        public float        changeSceneDelay;
    
        public override void InstallBindings()
        {
            Container.BindInstance(tips).WithId(TipsId);
            Container.BindInstance(progressSmoothFactor).WithId(ProgressSmoothFactorId);
            Container.BindInstance(changeSceneDelay).WithId(ChangeSceneDelayId);
        }
    }
}