using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin.CameraSystem
{
    [CreateAssetMenu(menuName = "Installers/CharacterCameraSettingInstaller")]
    public class CharacterCameraSettingInstaller :  ScriptableObjectInstaller<CharacterCameraSettingInstaller>
    {
        public const string TopClampID    = "CharacterCameraSettingInstaller.topClamp";
        public const string BottomClampID = "CharacterCameraSettingInstaller.bottomClamp";
        
        public const string LookYawMagnitudeID   = "CharacterCameraSettingInstaller.lookYawMagnitude";
        public const string LookPitchMagnitudeID = "CharacterCameraSettingInstaller.lookPitchMagnitude";
        
        public const string TargetZoomID       = "CharacterCameraSettingInstaller.targetZoom";
        public const string ZoomSmoothFactorID = "CharacterCameraSettingInstaller.zoomSmoothFactor";
        public const string ZoomMagnitudeID    = "CharacterCameraSettingInstaller.zoomMagnitude";
        public const string ZoomThreshHoldID   = "CharacterCameraSettingInstaller.zoomThreshHold";
        public const string ZoomMinValueID     = "CharacterCameraSettingInstaller.zoomMinValue";
        public const string ZoomMaxValueID     = "CharacterCameraSettingInstaller.zoomMaxValue";

        public float topClamp    = 70f;
        public float bottomClamp = -30f;

        public float lookYawMagnitude   = 0.1f;
        public float lookPitchMagnitude = 0.15f;

        public float targetZoom       = 2f;
        public float zoomSmoothFactor = 4f;
        public float zoomMagnitude    = 0.0015f;
        public float zoomThreshHold   = 0.1f;
        public float zoomMinValue     = 1f;
        public float zoomMaxValue     = 4f;
        
        public override void InstallBindings()
        {
            Container.BindInstance(topClamp).WithId(TopClampID);
            Container.BindInstance(bottomClamp).WithId(BottomClampID);
            
            Container.BindInstance(lookYawMagnitude).WithId(LookYawMagnitudeID);
            Container.BindInstance(lookPitchMagnitude).WithId(LookPitchMagnitudeID);
            
            Container.BindInstance(targetZoom).WithId(TargetZoomID);
            Container.BindInstance(zoomSmoothFactor).WithId(ZoomSmoothFactorID);
            Container.BindInstance(zoomMagnitude).WithId(ZoomMagnitudeID);
            Container.BindInstance(zoomThreshHold).WithId(ZoomThreshHoldID);
            Container.BindInstance(zoomMinValue).WithId(ZoomMinValueID);
            Container.BindInstance(zoomMaxValue).WithId(ZoomMaxValueID);
        }
    }
}
