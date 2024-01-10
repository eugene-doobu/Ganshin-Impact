using UnityEngine;

namespace GanShin.CameraSystem
{
    public class CharacterCameraSettingInstaller : ScriptableObject
    {
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
    }
}