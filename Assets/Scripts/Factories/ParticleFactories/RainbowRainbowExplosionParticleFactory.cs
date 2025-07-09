using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "RainbowRainbowExplosionParticle",
        menuName = "Factories/Particle/RainbowRainbowExplosionParticle")]
    public class RainbowRainbowExplosionParticleFactory: BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.RainbowRainbowExplosion;
        }
    }
}