using System.Collections.Generic;
using UnityEngine;

namespace GanShin
{
    public class LoadingSettingInstaller : ScriptableObject
    {
        public const string TipsId                 = "LoadingSceneInstaller.tips";
        public const string ProgressSmoothFactorId = "LoadingSceneInstaller.progressSmoothFactor";
        public const string ChangeSceneDelayId     = "LoadingSceneInstaller.changeSceneDelay";

        public List<string> tips;
        public float        progressSmoothFactor;
        public float        changeSceneDelay;
    }
}