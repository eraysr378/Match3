using System.Collections.Generic;
using Combinations;
using Factories.BaseFactories;
using Managers;
using Misc;
using ParticleEffects;
using UnityEngine;

namespace Factories.GeneralFactories
{
    public class GeneralParticleFactory : MonoBehaviour
    {
        [SerializeField] private List<BaseParticleFactory> particleFactoryList;

        private void OnEnable()
        {
            EventManager.OnParticleReturnToPool += ReturnToPool;
        }

        private void OnDisable()
        {
            EventManager.OnParticleReturnToPool -= ReturnToPool;
        }

        public PoolableParticle GetParticleBasedOnType(ParticleType particleType)
        {
            foreach (var factory in particleFactoryList)
            {
                if (factory.CanCreateParticle(particleType))
                {
                    return factory.Get();
                }
            }

            return null;
        }

        private void ReturnToPool(PoolableParticle particle)
        {
            foreach (var factory in particleFactoryList)
            {
                if (factory.CanCreateParticle(particle.GetParticleType()))
                {
                    factory.ReturnToPool(particle);
                    return;
                }
            }
        }
    }
}