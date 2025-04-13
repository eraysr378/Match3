using Factories.GeneralFactories;
using Managers;
using Misc;
using ParticleEffects;
using UnityEngine;

namespace Spawners
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private GeneralParticleFactory particleFactory;

        public void OnEnable()
        {
            EventManager.OnParticleSpawnRequested += SpawnParticle;
        }

        private void OnDisable()
        {
            EventManager.OnParticleSpawnRequested -= SpawnParticle;
        }


        private PoolableParticle SpawnParticle(ParticleType particleType,Transform parent,Vector3 position,Vector3 scale)
        {
            PoolableParticle particle = particleFactory.GetParticleBasedOnType(particleType);
            if (particle == null) return null;
            particle.transform.SetParent(parent);
            particle.transform.localScale = scale;
            particle.transform.position = position;
            return particle;
        }
    }
}