using System;
using Cysharp.Threading.Tasks;
using GanShin.Resource;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GanShin.Effect
{
    public enum eEffectType
    {
        RIKO_SWORD_HIT,
        RIKO_SWORD_ULTIMATE_HIT,
        MUSCLE_CAT_HIT,
        MUSCLE_CAT_ULTIMATE,
    }
    
    [UsedImplicitly]
    public class EffectManager : ManagerBase
    {
        private EffectManager() { }
        
        public ParticleSystem PlayEffect(eEffectType effectType, Vector3 position, bool isLooping = false)
        {
            var resourceManager = ProjectManager.Instance.GetManager<ResourceManager>();
            if (resourceManager == null)
            {
                GanDebugger.LogError($"Failed to get resource manager");
                return null;
            }
            
            var obj = resourceManager.Instantiate($"{effectType}.prefab");
            if (obj == null)
                return null;
            
            obj.transform.position = position;
            var particle = obj.GetComponent<ParticleSystem>();
            if(!isLooping) RemoveParticle(particle).Forget();
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
            float duration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
            foreach (var subParticles in particleSystem.GetComponentsInChildren<ParticleSystem>())
                duration = Mathf.Max(subParticles.main.duration + subParticles.main.startLifetime.constantMax);
            return duration;
        }
    }
}
