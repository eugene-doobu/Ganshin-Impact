using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GanShin.Director
{
    [Serializable]
    public class LightControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private LightControlBehaviour _template = new LightControlBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable  = ScriptPlayable<LightControlBehaviour>.Create(graph, _template);
            var behaviour = playable.GetBehaviour();
            return playable;
        }
    }
}
