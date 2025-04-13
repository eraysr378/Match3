using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "HexagonExplosionParticleFactory",
        menuName = "Factories/Particle/HexagonExplosionParticleFactory")]
    public class HexagonExplosionParticleFactory : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.HexagonExplosion;
        }
    }
}