using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/LoadingSceneInstaller")]
public class LoadingSceneInstaller : ScriptableObjectInstaller<LoadingSceneInstaller>
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