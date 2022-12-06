using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace GanShin.Director
{
    [TrackColor(241f/255f, 249f/255f, 99f/255f)]
    [TrackBindingType(typeof(Light))]
    [TrackClipType(typeof(LightControlClip))]
    public class LightControlTrack : TrackAsset
    {
        
    }
}
