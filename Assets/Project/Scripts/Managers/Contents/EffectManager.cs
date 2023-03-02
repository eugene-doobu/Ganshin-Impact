using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GanShin.Effect
{
    public enum eEffectType
    {
        RIKO_SWORD_HIT,
        RIKO_SWORD_ULTIMATE_HIT
    }
    
    [UsedImplicitly]
    public class EffectManager
    {
        // TODO: 풀링
        public ParticleSystem PlayEffect(eEffectType effectType, Vector3 position, bool isLooping = false)
        {
            var prefab = Resources.Load<GameObject>($"Effect/{effectType.ToString()}");
            var obj    = Object.Instantiate(prefab);
            obj.transform.position = position;
            var particle = obj.GetComponent<ParticleSystem>();
            if(!isLooping) RemoveParticle(particle).Forget();
            return particle;
        }

        private async UniTask RemoveParticle(ParticleSystem particleSystem)
        {
            float duration = particleSystem.main.duration;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            Object.Destroy(particleSystem.gameObject);
        }
    }
}
