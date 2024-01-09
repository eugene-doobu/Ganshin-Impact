using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GanShin.Sound
{
    [UsedImplicitly]
    public class SoundManager : ManagerBase
    {
        private const    string                        SoundObjName = "@Sound";
        private readonly Dictionary<string, AudioClip> _audioClips  = new();

        private readonly AudioSource[] _audioSources = new AudioSource[(int)Define.eSound.MaxCount];

        [UsedImplicitly]
        public SoundManager()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            var root = GameObject.Find(SoundObjName);
            if (root == null)
            {
                root = new GameObject { name = SoundObjName };
                Object.DontDestroyOnLoad(root);

                var soundNames = Enum.GetNames(typeof(Define.eSound));
                for (var i = 0; i < soundNames.Length - 1; i++)
                {
                    var go = new GameObject { name = soundNames[i] };
                    _audioSources[i]    = go.AddComponent<AudioSource>();
                    go.transform.parent = root.transform;
                }

                _audioSources[(int)Define.eSound.Bgm].loop = true;
            }

            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        private void OnSceneUnLoaded(Scene scene)
        {
            foreach (var audioSource in _audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }

            _audioClips.Clear();
        }

        public void Play(string path, Define.eSound type = Define.eSound.Effect, float pitch = 1.0f)
        {
            var audioClip = GetOrAddAudioClip(path, type);
            Play(audioClip, type, pitch);
        }

        public void Play(AudioClip audioClip, Define.eSound type = Define.eSound.Effect, float pitch = 1.0f)
        {
            if (audioClip == null)
                return;

            if (type == Define.eSound.Bgm)
            {
                var audioSource = _audioSources[(int)Define.eSound.Bgm];
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.pitch = pitch;
                audioSource.clip  = audioClip;
                audioSource.Play();
            }
            else
            {
                var audioSource = _audioSources[(int)Define.eSound.Effect];
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            }
        }

        private AudioClip GetOrAddAudioClip(string path, Define.eSound type = Define.eSound.Effect)
        {
            if (path.Contains("Sounds/") == false)
                path = $"Sounds/{path}";

            AudioClip audioClip = null;

            if (type == Define.eSound.Bgm)
            {
                audioClip = Resources.Load<AudioClip>(path);
            }
            else
            {
                if (_audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = Resources.Load<AudioClip>(path);
                    _audioClips.Add(path, audioClip);
                }
            }

            if (audioClip == null)
                GanDebugger.Log($"AudioClip Missing ! {path}");

            return audioClip;
        }
    }
}