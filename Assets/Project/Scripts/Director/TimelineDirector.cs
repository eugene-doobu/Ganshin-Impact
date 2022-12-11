using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

#nullable enable

namespace GanShin.Director
{
    public class TimelineDirector : GanshinDirector
    {
        protected TimelineAsset TimelineAsset { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            TimelineAsset = PlayableDirector!.playableAsset as TimelineAsset;
            if (TimelineAsset == null)
                GanDebugger.LogError(nameof(Director), "TimelineAsset is null");
        }

        #region Timeline

        

        #endregion Timeline
    }
}
