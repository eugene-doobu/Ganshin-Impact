using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GanShin.Director
{
    [TrackColor(0.9454092f, 0.9779412f, 0.3883002f)]
    [TrackBindingType(typeof(Light))]
    [TrackClipType(typeof(LightControlClip))]
    public class LightControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<LightControlBehaviour>.Create(graph, inputCount);
        }
    }
}