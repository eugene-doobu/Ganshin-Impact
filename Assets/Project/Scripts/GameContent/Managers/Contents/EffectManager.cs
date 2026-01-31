using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GanShin.Effect
{
    public enum eEffectType
    {
        RIKO_SWORD_HIT,
        RIKO_SWORD_ULTIMATE_HIT,
        RIKO_SKILL2,
        MUSCLE_CAT_HIT,
        MUSCLE_CAT_ULTIMATE,
        MUSCLE_CAT_SKILL1,
        MUSCLE_CAT_SKILL2,
        AI_PROJECTILE,
        AI_SKILL1,
        AI_SKILL2,
        AI_ULTIMATE,
    }

    [UsedImplicitly]
    public class EffectManager : ManagerBase
    {
        [UsedImplicitly]
        public EffectManager()
        {
        }

        public ParticleSystem PlayEffect(eEffectType effectType, Vector3 position, bool isLooping = false)
        {
            var obj = Util.Instantiate($"{effectType}.prefab");
            if (obj == null)
            {
                GanDebugger.LogWarning(nameof(EffectManager), $"Failed to instantiate {effectType}.prefab");
                return null;
            }

            obj.transform.position = position;
            var particle = obj.GetComponent<ParticleSystem>();
            if (particle != null && !isLooping) RemoveParticle(particle).Forget();
            return particle;
        }

        private async UniTask RemoveParticle(ParticleSystem particleSystem)
        {
            var duration = GetParticleDuration(particleSystem);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            Object.Destroy(particleSystem.gameObject);
        }

        private float GetParticleDuration(ParticleSystem particleSystem)
        {
            var duration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
            foreach (var subParticles in particleSystem.GetComponentsInChildren<ParticleSystem>())
                duration = Mathf.Max(subParticles.main.duration + subParticles.main.startLifetime.constantMax);
            return duration;
        }
    }
}