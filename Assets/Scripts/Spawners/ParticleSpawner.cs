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
            EventManager.RequestParticleSpawn += SpawnParticle;
        }

        private void OnDisable()
        {
            EventManager.RequestParticleSpawn -= SpawnParticle;
        }


        private PoolableParticle SpawnParticle(ParticleType particleType,Vector3 position)
        {
            PoolableParticle particle = particleFactory.GetParticleBasedOnType(particleType);
            if (particle == null) return null;
            particle.transform.position = position;
            return particle;
        }
    }
}