using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;

#nullable enable

namespace GanShin.Director
{
    [RequireComponent(typeof(PlayableDirector))]
    public class GanshinDirector : MonoBehaviour
    {
        public PlayableDirector? PlayableDirector { get; set; }
        
        #region MonoBehaviour

        protected virtual void Awake()
        {
            PlayableDirector = GetComponent<PlayableDirector>();
        }

        #endregion MonoBehaviour

        #region PlayableDirector

        public virtual void Play(IDirectorMessage? message = null)
        {
            PlayableDirector!.Play();
        }

        public virtual void Stop()
        {
            PlayableDirector!.Stop();
        }
        
        public virtual void Resume()
        {
            PlayableDirector!.Resume();
        }
        
        public virtual void Pause()
        {
            PlayableDirector!.Pause();
        }
        
        #endregion PlayableDirector
    }
}