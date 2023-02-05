using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/LoadingSceneInstaller")]
public class LoadingSceneInstaller : ScriptableObjectInstaller<LoadingSceneInstaller>
{
    public const string TipsId = "LoadingSceneInstaller.tips";
    
    public List<string> tips;
    
    public override void InstallBindings()
    {
        Container.BindInstance(tips).WithId(TipsId);
    }
}