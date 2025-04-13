using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "TriangleExplosionParticleFactory",
        menuName = "Factories/Particle/TriangleExplosionParticleFactory")]
    public class TriangleExplosionParticleFactory : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.TriangleExplosion;
        }
    }
}