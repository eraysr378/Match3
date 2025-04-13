using Misc;
using ParticleEffects;
using UnityEngine;

namespace Factories.BaseFactories
{
    public abstract class BaseParticleFactory : BasePoolableObjectFactory<PoolableParticle>
    {
        public abstract bool CanCreateParticle(ParticleType particleType);

    }
}