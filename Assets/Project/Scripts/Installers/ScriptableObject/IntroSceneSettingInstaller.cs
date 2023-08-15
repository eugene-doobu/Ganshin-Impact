using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.Director.IntroScene
{
    [CreateAssetMenu(menuName = "Installers/IntroSceneSettingInstaller")]
    public class IntroSceneSettingInstaller : ScriptableObjectInstaller<IntroSceneSettingInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}