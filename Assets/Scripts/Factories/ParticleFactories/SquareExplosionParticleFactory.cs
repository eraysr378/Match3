using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "SquareExplosionParticleFactory",
        menuName = "Factories/Particle/SquareExplosionParticleFactory")]
    public class SquareExplosionParticleFactory : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.SquareExplosion;
        }
    }
}