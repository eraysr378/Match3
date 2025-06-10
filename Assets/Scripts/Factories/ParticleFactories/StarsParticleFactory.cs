using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "StarsParticleFactory",
        menuName = "Factories/Particle/StarsParticleFactory")]
    public class StarsParticleFactory : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.Stars;
        }
    }
}