using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "BombExplosionParticle",
        menuName = "Factories/Particle/BombExplosionParticle")]
    public class BombExplosionParticle : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.BombExplosion;
        }
    }
}