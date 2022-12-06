using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GanShin.Director
{
    [Serializable]
    public class LightControlBehaviour : PlayableBehaviour
    {
        [SerializeField] private Color _color           = Color.white;
        [SerializeField] private float _intensity       = 1f;
        [SerializeField] private float _bounceIntensity = 1f;
        [SerializeField] private float _range           = 10f;

        private Light _light;
        
        private bool _firstFrameHappened;
        private Color _defaultColor;
        private float _defaultIntensity;
        private float _defaultBounceIntensity;
        private float _defaultRange;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var light = playerData as Light;
            if (light == null) return;
            
            CheckFirstFrame(light);
            
            var currentTime = (float)playable.GetTime() / (float)playable.GetDuration();
            
            light.color           = _color;
            light.intensity       = _intensity;
            light.bounceIntensity = _bounceIntensity;
            light.range           = _range;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        private void CheckFirstFrame(Light light)
        {
            if (_firstFrameHappened) return;
            _firstFrameHappened = true;

            _defaultColor           = light.color;
            _defaultIntensity       = light.intensity;
            _defaultBounceIntensity = light.bounceIntensity;
            _defaultRange           = light.range;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            _firstFrameHappened = false;

            if (_light == null)
                return;
            
            _light.color           = _defaultColor;
            _light.intensity       = _defaultIntensity;
            _light.bounceIntensity = _defaultBounceIntensity;
            _light.range           = _defaultRange;
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            _light = null;
        }
    }
}
