using System.Collections.Generic;
using UnityEngine;

namespace GanShin
{
    public class LoadingSettingInstaller : ScriptableObject
    {
        public List<string> tips;
        public float        progressSmoothFactor;
        public float        changeSceneDelay;
    }
}