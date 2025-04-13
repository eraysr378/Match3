using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "CircleExplosionParticleFactory",
        menuName = "Factories/Particle/CircleExplosionParticleFactory")]
    public class CircleExplosionParticleFactory : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.CircleExplosion;
        }
    }
}