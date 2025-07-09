using System;
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
        [SerializeField] private Transform particleParent;

        private void OnEnable()
        {
            EventManager.ReturnParticleToPool += ReturnToPool;
        }

        private void OnDisable()
        {
            EventManager.ReturnParticleToPool -= ReturnToPool;
            foreach (var factory in particleFactoryList)
            {
                factory.ResetPool();
            }
        }

        private void Awake()
        {
            foreach (var factory in particleFactoryList)
            {
                factory.Initialize(particleParent);
            }
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